class BorrowRepository : Repository<Borrow>
{
    public List<Borrow> SortDate()
    {
      return ReadAll().OrderBy(borrow => borrow.BorrowDate).ToList();
    }
}
