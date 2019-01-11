using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Tap.Data;
using Tap.Web.Models.Book;

namespace Tap.Web.Controllers
{
    public class BookController : Controller
    {
        private IRepository<Author> repoAuthor;
        private IRepository<Book> repoBook;
        public BookController(IRepository<Author> repoAuthor, IRepository<Book> repoBook)
        {
            this.repoAuthor = repoAuthor;
            this.repoBook = repoBook;
        }
        // GET: Book
        public IActionResult Index()
        {
            List<BookListingViewModel> model = new List<BookListingViewModel>();
            repoBook.GetAll().ToList().ForEach(b =>
            {
                BookListingViewModel book = new BookListingViewModel
                {
                    Id = b.Id,
                    BookName = b.Name,
                    Publisher = b.Publisher,
                    ISBN = b.ISBN
                };
                Author author = repoAuthor.Get(b.AuthorId);
                book.AuthorName = $"{author.FirstName} {author.LastName}";
                model.Add(book);
            });
            return View("Index", model);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
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
    }
}