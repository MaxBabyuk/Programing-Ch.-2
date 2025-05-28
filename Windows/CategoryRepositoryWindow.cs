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

public partial class CategoryRepositoryWindow : Window, INotifyPropertyChanged
{
    private readonly CategoryRepository _repository;
    private Category[] _selectedCategories = [];

    public new event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Category> Categories { get; set; } = [];

    public ObservableCollection<Category> SortedCategories { get; set; } = [];

    public Category[] SelectedCategories
    {
        get => _selectedCategories;
        set
        {
            _selectedCategories = value;
            OnPropertyChanged();
            UpdateButtonStates();
        }
    }

    public CategoryRepositoryWindow(CategoryRepository repository)
    {
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
        Categories.Clear();
        foreach (var category in _repository.ReadAll())
            Categories.Add(category);
    }

    private void CreateCategory(Category category)
    {
        _repository.Create(category);
        UpdateUI();
    }

    private void UpdateCategory(Category category)
    {
        _repository.Update(category);
        UpdateUI();
    }

    private void DeleteCategory(int categoryId)
    {
        _repository.Delete(categoryId);
        UpdateUI();
    }

    private void AddCategory_Click(object? sender, RoutedEventArgs e)
    {
        var currentCount = _repository.ReadAll().Count;
        var newCategory = new Category(currentCount + 1, $"Category{currentCount + 1}");
        CreateCategory(newCategory);
    }

    private void DeleteCategory_Click(object? sender, RoutedEventArgs e)
    {
        foreach (var category in SelectedCategories)
        {
            DeleteCategory(category.Id);
        }
    }

    private void SortByName_Click(object? sender, RoutedEventArgs e)
    {
        SortedCategories.Clear();
        foreach (var category in _repository.SortName())
            SortedCategories.Add(category);
    }

    private void BackToMain_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CategoryGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            SelectedCategories = dataGrid.SelectedItems.Cast<Category>().ToArray();
        }
    }

    private bool _isUpdatingCategory = false;

    private void CategoryGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (_isUpdatingCategory) return;

        if (e.EditAction != DataGridEditAction.Commit)
            return;

        if (e.Row.DataContext is not Category editedCategory)
            return;

        _isUpdatingCategory = true;

        try
        {
            Category updatedCategory = null!;

            if (e.EditingElement is TextBox textBox)
            {
                var column = e.Column.Header?.ToString();

                if (column == "Name")
                {
                    updatedCategory = new Category(editedCategory.Id, textBox.Text!);
                }
            }

            if (updatedCategory != null)
                UpdateCategory(updatedCategory);
        }
        finally
        {
            _isUpdatingCategory = false;
        }
    }

    private void UpdateButtonStates()
    {
        var deleteButton = this.FindControl<Button>("DeleteButton");
        if (deleteButton != null)
        {
            deleteButton.IsEnabled = SelectedCategories.Length > 0;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}