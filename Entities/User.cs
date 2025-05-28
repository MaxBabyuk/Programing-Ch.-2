namespace DBProject;

public class User : Entity
{
  public string Login { get; private set; }
  public string Password { get; private set; }
  public bool IsAdmin { get; private set; }
  public bool LoggedIn { get; private set; }

  public User(int id, string login, string password, bool isAdmin) : base(id)
  {
    Login = login;
    Password = password;
    IsAdmin = isAdmin;
  }

  public override string Stringify()
  {
    return $"User ({Id} {Login} {Password} {IsAdmin})";
  }

  public void LogIn(string password)
  {
    if (Password == password)
    {
      LoggedIn = true;
    }
  }

  public void LogOut()
  {
    LoggedIn = false;
  }

  public Borrow? MakeBorrow(Book book)
  {
    if (!LoggedIn)
    {
      return null;
    }
    return ((IBorrowable)book).MakeBorrow(this);
  }

  public Borrow? MakeBorrow(Book book, string password)
  {
    if (Password != password)
    {
      return null;
    }
    return ((IBorrowable)book).MakeBorrow(this);
  }


}
