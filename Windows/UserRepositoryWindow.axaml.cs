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

public partial class UserRepositoryWindow : Window, INotifyPropertyChanged
{
    private readonly UserRepository _repository;
    private User[] _selectedUsers = [];

    public new event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<User> Users { get; set; } = [];

    public ObservableCollection<User> SortedUsers { get; set; } = [];

    public User[] SelectedUsers
    {
        get => _selectedUsers;
        set
        {
            _selectedUsers = value;
            OnPropertyChanged();
            UpdateButtonStates();
        }
    }
    
    public bool IsAdmin { get; set; }

    public UserRepositoryWindow(UserRepository repository, bool admin)
    {
        IsAdmin = admin;
        InitializeComponent();
        _repository = repository;

        DataContext = this;

        UpdateUI();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void UpdateUI()
    {
        Users.Clear();
        foreach (var user in _repository.ReadAll())
            Users.Add(user);
    }

    private void CreateUser(User user)
    {
        _repository.Create(user);
        UpdateUI();
    }

    private void UpdateUser(User user)
    {
        _repository.Update(user);
        UpdateUI();
    }

    private void DeleteUser(int userId)
    {
        _repository.Delete(userId);

        UpdateUI();
    }

    private void AddUser_Click(object? sender, RoutedEventArgs e)
    {
        var currentCount = _repository.ReadAll().Count;
        var newUser = new User(currentCount + 1, $"user{currentCount + 1}", "password", false);
        CreateUser(newUser);
    }

    private void DeleteUser_Click(object? sender, RoutedEventArgs e)
    {
        foreach (var user in SelectedUsers)
        {
            DeleteUser(user.Id);
        }
    }

    private void SortById_Click(object? sender, RoutedEventArgs e)
    {
        SortedUsers.Clear();
        foreach (var user in _repository.SortId())
            SortedUsers.Add(user);
    }

    private void SortByLogin_Click(object? sender, RoutedEventArgs e)
    {
        SortedUsers.Clear();
        foreach (var user in _repository.SortLogin())
            SortedUsers.Add(user);
    }

    private void BackToMain_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void UserGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            SelectedUsers = dataGrid.SelectedItems.Cast<User>().ToArray();
        }
    }


    private bool _isUpdatingUser = false;

    private void UserGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (_isUpdatingUser) return;

        if (e.EditAction != DataGridEditAction.Commit)
            return;

        if (e.Row.DataContext is not User editedUser)
            return;

        _isUpdatingUser = true;

        try
        {
            User updatedUser = null!;

            if (e.EditingElement is TextBox textBox)
            {
                var column = e.Column.Header?.ToString();

                switch (column)
                {
                    case "Login":
                        updatedUser = new User(editedUser.Id, textBox.Text!, editedUser.Password, editedUser.IsAdmin);
                        break;
                    case "Password":
                        updatedUser = new User(editedUser.Id, editedUser.Login, textBox.Text!, editedUser.IsAdmin);
                        break;
                    case "Is Admin":
                        updatedUser = new User(editedUser.Id, editedUser.Login, editedUser.Password, bool.Parse(textBox.Text!));
                        break;
                }
            }

            if (updatedUser != null)
                UpdateUser(updatedUser);
        }
        finally
        {
            _isUpdatingUser = false;
        }
    }
    private void UpdateButtonStates()
    {
        var deleteButton = this.FindControl<Button>("DeleteButton");
        if (deleteButton != null)
        {
            deleteButton.IsEnabled = SelectedUsers.Length > 0;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}