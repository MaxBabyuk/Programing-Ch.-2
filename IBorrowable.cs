namespace DBProject;

public interface IBorrowable
{
    Borrow? MakeBorrow(User user);
    void Return();
    bool IsBorrowed { get; }
}
