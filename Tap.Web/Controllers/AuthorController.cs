using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Tap.Data;
using Tap.Web.Models;
using Tap.Web.Models.Book;

namespace Tap.Web.Controllers
{
    public class AuthorController : Controller
    {
        private IRepository<Author> repoAuthor;
        private IRepository<Book> repoBook;
        public AuthorController(IRepository<Author> repoAuthor, IRepository<Book> repoBook)
        {
            this.repoAuthor = repoAuthor;
            this.repoBook = repoBook;
        }
        // GET: Author
        [HttpGet]
        public IActionResult Index()
        {
            List<AuthorListingViewModel> model = new List<AuthorListingViewModel>();
            repoAuthor.GetAll().ToList().ForEach(a =>
            {
                AuthorListingViewModel author = new AuthorListingViewModel
                {
                    Id = a.Id,
                    Name = $"{a.FirstName} {a.LastName}",
                    Email = a.Email
                };
                author.TotalBooks = repoBook.GetAll().Count(x => x.AuthorId == a.Id);
                model.Add(author);
            });
            return View("Index", model);
        }

        // GET: Author/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Author/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        public ActionResult AddAuthor(AuthorBookViewModel model)
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
            return RedirectToAction("Create");
        }


        // GET: Author/Edit/5
        [HttpGet]
        public IActionResult Edit(long id)
        {
            AuthorViewModel model = new AuthorViewModel();
            Author author = repoAuthor.Get(id);
            if (author != null)
            {
                model.FirstName = author.FirstName;
                model.LastName = author.LastName;
                model.Email = author.Email;
            }
            return View("Edit", model);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, AuthorViewModel model)
        {
            try
            {
                Author author = repoAuthor.Get(id);
                if (author != null)
                {
                    author.FirstName = model.FirstName;
                    author.LastName = model.LastName;
                    author.Email = model.Email;
                    author.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    author.ModifiedDate = DateTime.UtcNow;
                    repoAuthor.Update(author);
                }
                return RedirectToAction("Edit");
            }
            catch
            {
                return View();
            }
        }


        // GET: Author/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ViewResult Book(long id)
        {
            BookViewModel model = new BookViewModel();
            return View("Book", model);
        }
        [HttpPost]
        public IActionResult AddBook(long id, BookViewModel model)
        {
            Book book = new Book
            {
                AuthorId = id,
                Name = model.BookName,
                ISBN = model.ISBN,
                Publisher = model.Publisher,
                IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                AddedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            var job = BackgroundJob.Schedule(() => repoBook.Insert(book), TimeSpan.FromMinutes(2));
            return RedirectToAction("Book");
        }
    }
}