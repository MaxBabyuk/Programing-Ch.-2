using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBProject;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private UserRepository _userRepository = new();
    private CategoryRepository _categoryRepository = new();
    private BookRepository _bookRepository = new();
    private BorrowRepository _borrowRepository = new();

    public event PropertyChangedEventHandler? PropertyChanged;
    private User? _registeredUser;
    public User? RegisteredUser
    {
        get => _registeredUser;
        set
        {
            _registeredUser = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RegisteredUser)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLogged)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdmin)));
        }
    }
    public bool IsLogged => RegisteredUser != null;
    public bool IsAdmin => RegisteredUser?.IsAdmin ?? false;


    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _userRepository.Register("admin", "admin", true);


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

    private void OpenAuthWindow_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var authWindow = new AuthWindow(_userRepository, (u) => { RegisteredUser = u; });
        authWindow.Show();
    }

    private void OpenUserRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new UserRepositoryWindow(_userRepository, IsAdmin);
        userRepoWindow.Show();
    }

    private void OpenCategoryRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new CategoryRepositoryWindow(_categoryRepository, IsAdmin);
        userRepoWindow.Show();
    }

    private void OpenBookRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new BookRepositoryWindow(_bookRepository, _categoryRepository, IsAdmin);
        userRepoWindow.Show();
    }

    private void OpenBorrowRepository_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var userRepoWindow = new BorrowRepositoryWindow(_borrowRepository, _userRepository, _bookRepository, IsAdmin);
        userRepoWindow.Show();
    }
}