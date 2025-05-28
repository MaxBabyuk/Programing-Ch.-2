namespace DBProject;

public class Book : Entity, IBorrowable
{
    public string Title { get; private set; }
    public Category BookCategory { get; private set; }
    public string Author { get; private set; }
    public Borrow? BookBorrow { get; private set; }
    
    public bool IsBorrowed => BookBorrow != null;

    public Book(int id, string title, string author, Category category) : base(id)
    {
        Title = title;
        Author = author;
        BookCategory = category;
    }

    public override string Stringify()
    {
        return $"Book ({Id} {Title} {BookCategory})";
    }

    public Borrow? MakeBorrow(User user)
    {
      if (IsBorrowed)
      {
        return null;
      }
      BookBorrow = new Borrow(0, this, user);
      return BookBorrow;
    }

    public void Return()
    {
      BookBorrow = null;
    }

    public override bool IsSame(object obj)
    {
      return obj is Book book && book.Id == Id;
    }
}
