using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;

namespace DBProject;

public partial class AuthWindow : Window, INotifyPropertyChanged
{
    private UserRepository _userRepository;
    private Action<User> _onLoginSuccess;

    private string _username = "";
    private string _password = "";
    private string _confirmPassword = "";
    
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            RaisePropertyChanged(nameof(Username));
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            RaisePropertyChanged(nameof(Password));
        }
    }
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            RaisePropertyChanged(nameof(ConfirmPassword));
        }
    }

    public AuthWindow(UserRepository userRepository, Action<User> onLoginSuccess)
    {
        InitializeComponent();
        _userRepository = userRepository;
        _onLoginSuccess = onLoginSuccess;
        
        DataContext = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {

        var _user = _userRepository.LogIn(Username, Password);
        if (_user != null)
        {
            // Login successful, navigate to next page or perform some action
            await MessageBoxManager.GetMessageBoxStandard("Login", "Login successful!").ShowAsync();
            _onLoginSuccess(_user);
            Close();
        }
        else
        {
            await MessageBoxManager.GetMessageBoxStandard("Login", "Invalid username or password").ShowAsync();
        }
    }

    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        if (Password == ConfirmPassword)
        {
            if (_userRepository.Register(Username, Password))
            {
                await MessageBoxManager.GetMessageBoxStandard("Register", "Registration successful!").ShowAsync();
            }
            else
            {

                await MessageBoxManager.GetMessageBoxStandard("Register", "Username already exists").ShowAsync();
            }
        }
        else
        {
            await MessageBoxManager.GetMessageBoxStandard("Register", "Passwords do not match").ShowAsync();
        }
    }

public event PropertyChangedEventHandler PropertyChanged;

private void RaisePropertyChanged(string propertyName)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
}