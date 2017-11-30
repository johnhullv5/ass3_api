using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _300858525_Jiang__ASS.Models;
using _300858525_Jiang__ASS.Services;
using Microsoft.AspNetCore.Http;

namespace _300858525_Jiang__ASS.Controllers
{
    public class MoviesController : Controller
    {
        
            /// <summary>
            /// How many books should we display on each page of the index?
            /// </summary>
            private const int _pageSize = 10;

            private readonly IMovieStore _store;
            private readonly ImageUploader _imageUploader;

            public MoviesController()
            {
                _store = new DatastoreMovieStore("assig1hao");
                _imageUploader = new ImageUploader("book_hao");
            }

            // GET: Books
            public ActionResult Index(string nextPageToken)
            {
                return View(new ViewModels.Movies.Index()
                {
                    MovieList = _store.List(_pageSize, nextPageToken)
                });
            }

            // GET: Books/Details/5
            public ActionResult Details(long? id)
            {
                if (id == null)
                {
                  return StatusCode(404);
            }

                Movie movie = _store.Read((long)id);
                if (movie == null)
                {
                    return StatusCode(404);
            }

                return View(movie);
            }

            // GET: Movies/Create
            public ActionResult Create()
            {
                return ViewForm("Create", "Create");
            }

        // [START create]
        // POST: Movies/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Create(Movie movie, IFormFile image)
            {
                if (ModelState.IsValid)
                {
                    _store.Create(movie);
                    // If book cover image submitted, save image to Cloud Storage
                    if (image != null)
                    {
                    var imageUrl = await _imageUploader.UploadImage(image, movie.Id);
                    movie.ImageUrl = imageUrl;
                    _store.Update(movie);
                    }
                    return RedirectToAction("Details", new { id = movie.Id });
                }
                return ViewForm("Create", "Create", movie);
            }
            // [END create]

            /// <summary>
            /// Dispays the common form used for the Edit and Create pages.
            /// </summary>
            /// <param name="action">The string to display to the user.</param>
            /// <param name="movie">The asp-action value.  Where will the form be submitted?</param>
            /// <returns>An IActionResult that displays the form.</returns>
            private ActionResult ViewForm(string action, string formAction, Movie movie = null)
            {
                var form = new ViewModels.Movies.Form()
                {
                    Action = action,
                    Movie = movie ?? new Movie(),
                    IsValid = ModelState.IsValid,
                    FormAction = formAction
                };
                return View("/Views/Movies/Form.cshtml", form);
            }

            // GET: Books/Edit/5
            public ActionResult Edit(long? id)
            {
                if (id == null)
                {
                    return StatusCode(404);
            }

                Movie movie = _store.Read((long)id);
                if (movie == null)
                {
                    return StatusCode(404);
            }
                return ViewForm("Edit", "Edit", movie);
            }
        // GET: Books/Download/5
        public async Task<ActionResult> Download(long? id)
        {
            if (id == null)
            {
                return null;
            }
            try {
                _imageUploader.DownloadObject("book_hao", id.ToString(), "c:\\");
            }
            catch (Exception e)
            {
                return null;
            }



            Movie movie = _store.Read((long)id);
            if (movie == null)
            {
                return null;
            }
            else
            {
                return  RedirectToAction("Index", null);
            }
            //return ViewForm("Edit", "Edit", movie);
        }

        public async Task<ActionResult> AddComment(long? id,string content)
        {
            if (id == null)
            {
                return null;
            }
            try
            {
                _imageUploader.DownloadObject("book_hao", id.ToString(), "c:\\");
            }
            catch (Exception e)
            {
                return null;
            }



            Movie movie = _store.Read((long)id);
            if (movie == null)
            {
                return null;
            }
            else
            {
                return RedirectToAction("Index", null);
            }
            //return ViewForm("Edit", "Edit", movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Edit(Movie movie, long id, IFormFile image)
            {
                if (ModelState.IsValid)
                {
                    movie.Id = id;
                    if (image != null)
                    {
                        movie.ImageUrl = await _imageUploader.UploadImage(image, movie.Id);
                    }
                    _store.Update(movie);
                    return RedirectToAction("Details", new { id = movie.Id });
                }
                return ViewForm("Edit", "Edit", movie);
            }

            // POST: Movies/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Delete(long id)
            {
                // Delete book cover image from Cloud Storage if ImageUrl exists
                string imageUrlToDelete = _store.Read((long)id).ImageUrl;
                if (imageUrlToDelete != null)
                {
                    await _imageUploader.DeleteUploadedImage(id);
                }
                _store.Delete(id);
                return RedirectToAction("Index");
            }
        }
    
}