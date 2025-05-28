namespace DBProject;
public class BookModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }

    public string Category { get; set; }
    
    public BookModel(int id, string title, string author, string category)
    {
        Id = id;
        Title = title;
        Author = author;
        Category = category;
    }
}