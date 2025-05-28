using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBProject;

public partial class MainWindow : Window
{
    private UserRepository _userRepository = new();
    private CategoryRepository _categoryRepository = new();
    private BookRepository _bookRepository = new();
    private BorrowRepository _borrowRepository = new();


    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _userRepository.Create(new User(1, "alice", "password1", true));
        _userRepository.Create(new User(2, "bob", "password2", false));
        _userRepository.Create(new User(3, "charlie", "password3", true));

        _categoryRepository.Create(new Category(1, "Fiction"));
        _categoryRepository.Create(new Category(2, "Educational"));
        _categoryRepository.Create(new Category(3, "Drama"));

        _bookRepository.Create(new Book(1, "The Great Gatsby", "F. Scott Fitzgerald", _categoryRepository.ReadByName("Fiction")!));
        _bookRepository.Create(new Book(2, "To Kill a Mockingbird", "Harper Lee", _categoryRepository.ReadByName("Fiction")!));
        _bookRepository.Create(new Book(3, "1984", "George Orwell", _categoryRepository.ReadByName("Drama")!));

        _borrowRepository.Create(new Borrow(1, _bookRepository.Read(1)!, _userRepository.Read(1)!));
        _borrowRepository.Create(new Borrow(2, _bookRepository.Read(2)!, _userRepository.Read(2)!));
        _borrowRepository.Create(new Borrow(3, _bookRepository.Read(3)!, _userRepository.Read(3)!));
    }

    private void OpenUserRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new UserRepositoryWindow(_userRepository);
        userRepoWindow.Show();
    }

    private void OpenCategoryRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new CategoryRepositoryWindow(_categoryRepository);
        userRepoWindow.Show();
    }

    private void OpenBookRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new BookRepositoryWindow(_bookRepository, _categoryRepository);
        userRepoWindow.Show();
    }

    private void OpenBorrowRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new BorrowRepositoryWindow(_borrowRepository, _userRepository, _bookRepository);
        userRepoWindow.Show();
    }
}