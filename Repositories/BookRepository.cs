class BookRepository : Repository<Book>
{
  public List<Book> SortName()
  {
    return ReadAll().OrderBy(book => book.Title).ToList();
  }
}
