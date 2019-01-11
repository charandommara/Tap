using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tap.Web.Models.Book
{
    public class BookListingViewModel
    {
        public long Id
        {
            get;
            set;
        }
        public string BookName
        {
            get;
            set;
        }
        public string AuthorName
        {
            get;
            set;
        }
        public string ISBN
        {
            get;
            set;
        }
        public string Publisher
        {
            get;
            set;
        }
    }
}
