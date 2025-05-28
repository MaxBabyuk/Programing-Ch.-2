using System.Collections.Generic;
using System.Linq;

namespace DBProject;

public class BorrowRepository : Repository<Borrow>
{
    public List<Borrow> SortDate()
    {
      return ReadAll().OrderBy(borrow => borrow.BorrowDate).ToList();
    }
}
