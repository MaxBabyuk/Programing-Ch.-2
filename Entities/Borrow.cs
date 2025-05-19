class Borrow : Entity
{
    public Book TargetBook { get; private set; }
  public User TargetUser { get; private set; }
  public DateTime BorrowDate { get; private set; }

    public Borrow(int id, Book book, User user) : base(id)
    {
      TargetBook = book;
      TargetUser = user;
      BorrowDate = DateTime.Now;
    }

    public override string Stringify()
    {
        throw new NotImplementedException();
    }
}
