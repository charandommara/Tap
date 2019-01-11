using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Tap.Data;
using Tap.Web.Models;
using Tap.Web.Models.Book;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tap.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IRepository<Author> repoAuthor;
        private IRepository<Book> repoBook;
        public ValuesController(IRepository<Author> repoAuthor, IRepository<Book> repoBook)
        {
            this.repoAuthor = repoAuthor;
            this.repoBook = repoBook;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("[action]")]
        public List<BookViewModel> GetBooks()
        {
            var book = repoBook.GetAll();
            List<BookViewModel> lBooks = new List<BookViewModel>();
            BookViewModel bookView = new BookViewModel();
            foreach (var b in book)
            {
                bookView = new BookViewModel
                {
                    BookName = b.Name,
                    ISBN = b.ISBN,
                    Publisher = b.Publisher
                };
                lBooks.Add(bookView);
            }
            return lBooks;
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        public AuthorBookViewModel GetAuthor(long Id)
        {
            var author = repoAuthor.Get(Id);
            AuthorBookViewModel authorBook = new AuthorBookViewModel ();
            authorBook.Id = author.Id;
            authorBook.FirstName = author.FirstName;
            authorBook.LastName = author.LastName;
            authorBook.Email = author.Email;
            var book = repoBook.Find(f => f.AuthorId == Id).ToList();
           
            
                authorBook.BookName = book[0].Name;
                authorBook.ISBN = book[0].ISBN;
                authorBook.Publisher = book[0].Publisher;
            
            return authorBook;
        }



        [Route("[action]/{Id}")]
        [HttpGet]
        public string Job(string id)
        {
            var jobId = BackgroundJob.Enqueue(() => Console.Write(id));
            return jobId;
        }

        [Route("[action]/{Id}/{days}")]
        [HttpGet]
        public string ScheduleJob(string id, double days)
        {

            var jobId = BackgroundJob.Schedule(() => Console.Write("Checking schedule"), TimeSpan.FromDays(days));
            return jobId;
        }

        // POST api/<controller>
        [ActionName("CreateAuthor")]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("CreateAuthor")]
        public string CreateAuthor([FromBody]AuthorBookViewModel model)
        {
            Author author = new Author
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                AddedDate = DateTime.UtcNow,
                IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                ModifiedDate = DateTime.UtcNow,
                Books = new List<Book> {
                new Book {
                    Name = model.BookName,
                        ISBN = model.ISBN,
                        Publisher = model.Publisher,
                        IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        AddedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                }}
            };

            var jobId = BackgroundJob.Enqueue(() => repoAuthor.Insert(author));
            BackgroundJob.Schedule(() => MailHelper.SendEmail("somemail@gmail.com", model.Email, "somemail@gmail.com", "somemail", "Author Registerd", "Thank you for registring."), TimeSpan.FromMinutes(5));
            return author.Id.ToString();
        }

    }
}
