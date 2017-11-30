using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _300858525_Jiang__ASS.Models
{
   
    /// <summary>
    /// An interface for storing books.  Can be implemented by a database,
    /// Google Datastore, etc.
    /// </summary>
    public interface IMovieStore//: Unity.IUnityContainer
    {
        /// <summary>
        /// Creates a new book.  The Id of the book will be filled when the
        /// function returns.
        /// </summary>
        void Create(Movie movie);

        Movie Read(long id);

        void Update(Movie movie);

        void Delete(long id);

        MovieList List(int pageSize, string nextPageToken);
    }

}
