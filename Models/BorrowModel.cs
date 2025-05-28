namespace DBProject;
public class BorrowModel
{
    public int Id { get; set; }
    public string User { get; set; }
    public string Book { get; set; } 
    public BorrowModel(int id, string user, string book)
    {
        Id = id;
        User = user;
        Book = book;
    }
}