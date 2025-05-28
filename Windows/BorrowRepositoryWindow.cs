using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DBProject;

public partial class BorrowRepositoryWindow : Window, INotifyPropertyChanged
{
    private readonly BorrowRepository _repository;
    private readonly UserRepository _userRepository;
    private readonly BookRepository _bookRepository;
    private BorrowModel[] _selectedBorrows = [];

    public new event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<BorrowModel> Borrows { get; set; } = [];

    public ObservableCollection<BorrowModel> SortedBorrows { get; set; } = [];

    public BorrowModel[] SelectedBorrows
    {
        get => _selectedBorrows;
        set
        {
            _selectedBorrows = value;
            OnPropertyChanged();
            UpdateButtonStates();
        }
    }
    
    public bool IsAdmin { get; set; }

    public BorrowRepositoryWindow(BorrowRepository repository, UserRepository userRepository, BookRepository bookRepository, bool admin)
    {
        IsAdmin = admin;
        InitializeComponent();
        _repository = repository;
        _bookRepository = bookRepository;
        _userRepository = userRepository;

        DataContext = this;
        UpdateUI();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void UpdateUI()
    {
        Borrows.Clear();
        foreach (var Borrow in _repository.ReadAll())
            Borrows.Add(new BorrowModel(Borrow.Id, Borrow.TargetUser.Login, Borrow.TargetBook.Title));
    }

    private void CreateBorrow(Borrow Borrow)
    {
        _repository.Create(Borrow);
        UpdateUI();
    }

    private void UpdateBorrow(Borrow Borrow)
    {
        _repository.Update(Borrow);
        UpdateUI();
    }

    private void DeleteBorrow(int BorrowId)
    {
        _repository.Delete(BorrowId);
        UpdateUI();
    }

    private void AddBorrow_Click(object? sender, RoutedEventArgs e)
    {
        var currentCount = _repository.ReadAll().Count;
        var newBorrow = new Borrow(currentCount + 1, _bookRepository.ReadAll()[0], _userRepository.ReadAll()[0]);
        CreateBorrow(newBorrow);
    }

    private void DeleteBorrow_Click(object? sender, RoutedEventArgs e)
    {
        foreach (var Borrow in SelectedBorrows)
        {
            DeleteBorrow(Borrow.Id);
        }
    }

    private void BackToMain_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BorrowGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            SelectedBorrows = dataGrid.SelectedItems.Cast<BorrowModel>().ToArray();
        }
    }

    private bool _isUpdatingBorrow = false;

    private void BorrowGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (_isUpdatingBorrow) return;

        if (e.EditAction != DataGridEditAction.Commit)
            return;

        if (e.Row.DataContext is not BorrowModel editedBorrow)
            return;

        _isUpdatingBorrow = true;

        try
        {
            Borrow updatedBorrow = null!;

            if (e.EditingElement is TextBox textBox)
            {
                var column = e.Column.Header?.ToString();

                switch (column)
                {
                    case "User":
                        updatedBorrow = new Borrow(editedBorrow.Id, _bookRepository.ReadByTitle(editedBorrow.Book)!, _userRepository.ReadByLogin(editedBorrow.User)!);
                        break;
                    case "Book":
                        updatedBorrow = new Borrow(editedBorrow.Id, _bookRepository.ReadByTitle(editedBorrow.Book)!, _userRepository.ReadByLogin(editedBorrow.User)!);
                        break;
                }
            }

            if (updatedBorrow != null)
                UpdateBorrow(updatedBorrow);
        }
        finally
        {
            _isUpdatingBorrow = false;
        }
    }

    private void UpdateButtonStates()
    {
        var deleteButton = this.FindControl<Button>("DeleteButton");
        if (deleteButton != null)
        {
            deleteButton.IsEnabled = SelectedBorrows.Length > 0;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}