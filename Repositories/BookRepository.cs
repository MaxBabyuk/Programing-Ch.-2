using System.Collections.Generic;
using System.Linq;

namespace DBProject;

public class BookRepository : Repository<Book>
{
    public List<Book> SortName()
    {
        return ReadAll().OrderBy(book => book.Title).ToList();
    }
  
  public Book? ReadByTitle(string title)
  {
    return ReadAll().FirstOrDefault(book => book.Title == title);
  }
}
