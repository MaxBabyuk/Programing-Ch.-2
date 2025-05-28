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

public partial class BookRepositoryWindow : Window, INotifyPropertyChanged
{
    private readonly BookRepository _repository;
    private readonly CategoryRepository _categoryRepository;
    private BookModel[] _selectedBooks = [];

    public new event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<BookModel> Books { get; set; } = [];

    public ObservableCollection<BookModel> SortedBooks { get; set; } = [];
    
    public bool IsAdmin { get; set; }

    public BookModel[] SelectedBooks
    {
        get => _selectedBooks;
        set
        {
            _selectedBooks = value;
            OnPropertyChanged();
            UpdateButtonStates();
        }
    }

    public BookRepositoryWindow(BookRepository repository, CategoryRepository categoryRepository, bool admin)
    {
        IsAdmin = admin;
        InitializeComponent();
        _repository = repository;
        _categoryRepository = categoryRepository;

        DataContext = this;
        UpdateUI();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void UpdateUI()
    {
        Books.Clear();
        foreach (var Book in _repository.ReadAll())
            Books.Add(new BookModel(Book.Id, Book.Title, Book.Author, Book.BookCategory.Name));
    }

    private void CreateBook(Book Book)
    {
        _repository.Create(Book);
        UpdateUI();
    }

    private void UpdateBook(Book Book)
    {
        _repository.Update(Book);
        UpdateUI();
    }

    private void DeleteBook(int BookId)
    {
        _repository.Delete(BookId);
        UpdateUI();
    }

    private void AddBook_Click(object? sender, RoutedEventArgs e)
    {
        var currentCount = _repository.ReadAll().Count;
        var newBook = new Book(currentCount + 1, $"Book{currentCount + 1}", "Author", _categoryRepository.ReadAll()[0]);
        CreateBook(newBook);
    }

    private void DeleteBook_Click(object? sender, RoutedEventArgs e)
    {
        foreach (var Book in SelectedBooks)
        {
            DeleteBook(Book.Id);
        }
    }

    private void SortByTitle_Click(object? sender, RoutedEventArgs e)
    {
        SortedBooks.Clear();
        foreach (var Book in _repository.SortName())
            SortedBooks.Add(new BookModel(Book.Id, Book.Title, Book.Author, Book.BookCategory.Name));
    }

    private void BackToMain_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BookGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            SelectedBooks = dataGrid.SelectedItems.Cast<BookModel>().ToArray();
        }
    }

    private bool _isUpdatingBook = false;

    private void BookGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (_isUpdatingBook) return;

        if (e.EditAction != DataGridEditAction.Commit)
            return;

        if (e.Row.DataContext is not BookModel editedBook)
            return;

        _isUpdatingBook = true;

        try
        {
            Book updatedBook = null!;

            if (e.EditingElement is TextBox textBox)
            {
                var column = e.Column.Header?.ToString();

                switch (column)
                {
                    case "Title":
                        updatedBook = new Book(editedBook.Id, textBox.Text!, editedBook.Author, _categoryRepository.ReadByName(editedBook.Category)!);
                        break;
                    case "Author":
                        updatedBook = new Book(editedBook.Id, editedBook.Title, textBox.Text!, _categoryRepository.ReadByName(editedBook.Category)!);
                        break;
                    case "Category":
                        updatedBook = new Book(editedBook.Id, editedBook.Title, editedBook.Author, _categoryRepository.ReadByName(textBox.Text!)!);
                        break;
                }
            }

            if (updatedBook != null)
                UpdateBook(updatedBook);
        }
        finally
        {
            _isUpdatingBook = false;
        }
    }

    private void UpdateButtonStates()
    {
        var deleteButton = this.FindControl<Button>("DeleteButton");
        if (deleteButton != null)
        {
            deleteButton.IsEnabled = SelectedBooks.Length > 0;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}