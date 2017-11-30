using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Unity;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace _300858525_Jiang__ASS.Models

{
    public class DbMovieStore:IMovieStore
    {
        private readonly ApplicationDbContext _dbcontext;

        public DbMovieStore(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

       // public IUnityContainer Parent => throw new NotImplementedException();

        //public IEnumerable<IContainerRegistration> Registrations => throw new NotImplementedException();

        //public IUnityContainer AddExtension(UnityContainerExtension extension)
        //{
        //    throw new NotImplementedException();
        //}

        //public object BuildUp(Type type, object existing, string name, params ResolverOverride[] resolverOverrides)
        //{
        //    throw new NotImplementedException();
        //}

        //public object Configure(Type configurationInterface)
        //{
        //    throw new NotImplementedException();
        //}

        // [START create]
        public void Create(Movie movie)
        {
            var trackMovie = _dbcontext.Movies.Add(movie);
            _dbcontext.SaveChanges();
            movie.Id = trackMovie.Id;
        }

        //public IUnityContainer CreateChildContainer()
        //{
        //    throw new NotImplementedException();
        //}

        // [END create]
        public void Delete(long id)
        {
            Movie movie = _dbcontext.Movies.Single(m => m.Id == id);
            _dbcontext.Movies.Remove(movie);
            _dbcontext.SaveChanges();
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        // [START list]
        public MovieList List(int pageSize, string nextPageToken)
        {
            IQueryable<Movie> query = _dbcontext.Movies.OrderBy(movie => movie.Id);
            if (nextPageToken != null)
            {
                long previousBookId = long.Parse(nextPageToken);
                query = query.Where(movie => movie.Id > previousBookId);
            }
            var movies = query.Take(pageSize).ToArray();
            return new MovieList()
            {
                Movies = movies,
                NextPageToken = movies.Count() == pageSize ? movies.Last().Id.ToString() : null
            };
        }
        // [END list]

        public Movie Read(long id)
        {
            return _dbcontext.Movies.Single(m => m.Id == id);
        }

        //public IUnityContainer RegisterInstance(Type type, string name, object instance, LifetimeManager lifetime)
        //{
        //    throw new NotImplementedException();
        //}

        //public IUnityContainer RegisterType(Type typeFrom, Type typeTo, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        //{
        //    throw new NotImplementedException();
        //}

        //public IUnityContainer RemoveAllExtensions()
        //{
        //    throw new NotImplementedException();
        //}

        //public object Resolve(Type type, string name, params ResolverOverride[] resolverOverrides)
        //{
        //    throw new NotImplementedException();
        //}

        public void Update(Movie movie)
        {
            _dbcontext.Entry(movie).State = System.Data.Entity.EntityState.Modified;
            _dbcontext.SaveChanges();
        }
    }
}
