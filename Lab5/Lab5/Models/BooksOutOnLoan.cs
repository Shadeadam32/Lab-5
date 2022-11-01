using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    internal class BooksOutOnLoan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string DateIssued { get; set; }
        public string DueDate { get; set; }
        public string DateReturned { get; set; }
        public int UserId { get; set; }

    }
}
