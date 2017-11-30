using System;
using System.Collections.Generic;
using System.Linq;


using Google.Cloud.Datastore.V1;
using Google.Protobuf;
using Unity;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace _300858525_Jiang__ASS.Models
{
    
    public static class DatastoreMovieStoreExtensionMethods
    {
        /// <summary>
        /// Make a datastore key given a movie's id.
        /// </summary>
        /// <param name="id">A movie's id.</param>
        /// <returns>A datastore key.</returns>
        public static Key ToKey(this long id) =>
            new Key().WithElement("Movie", id);

        /// <summary>
        /// Make a movie id given a datastore key.
        /// </summary>
        /// <param name="key">A datastore key</param>
        /// <returns>A movie id.</returns>
        public static long ToId(this Key key) => key.Path.First().Id;

        /// <summary>
        /// Create a datastore entity with the same values as movie.
        /// </summary>
        /// <param name="movie">The movie to store in datastore.</param>
        /// <returns>A datastore entity.</returns>
        /// [START toentity]
        public static Entity ToEntity(this Movie movie) => new Entity()
        {
            Key = movie.Id.ToKey(),
            ["Title"] = movie.Title,
            ["Director"] = movie.Director,
            ["PublishedDate"] = movie.PublishedDate?.ToUniversalTime(),
            ["ImageUrl"] = movie.ImageUrl,
            ["Description"] = movie.Description,
            ["CreateById"] = movie.CreatedById
        };
        // [END toentity]

        /// <summary>
        /// Unpack a movie from a datastore entity.
        /// </summary>
        /// <param name="entity">An entity retrieved from datastore.</param>
        /// <returns>A movie.</returns>
        public static Movie ToMovie(this Entity entity) => new Movie()
        {
            Id = entity.Key.Path.First().Id,
            Title = (string)entity["Title"],
            Director = (string)entity["Director"],
            PublishedDate = (DateTime?)entity["PublishedDate"],
            ImageUrl = (string)entity["ImageUrl"],
            Description = (string)entity["Description"],
            CreatedById = (string)entity["CreatedById"]
        };
    }

        public class DatastoreMovieStore : IMovieStore
        {
            private readonly string _projectId;
            private readonly DatastoreDb _db;

            /// <summary>
            /// Create a new datastore-backed bookstore.
            /// </summary>
            /// <param name="projectId">Your Google Cloud project id</param>
            public DatastoreMovieStore(string projectId)
            {
                _projectId = projectId;
                _db = DatastoreDb.Create(_projectId);
            }

        //public IUnityContainer Parent => throw new NotImplementedException();

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
                var entity = movie.ToEntity();
                entity.Key = _db.CreateKeyFactory("Movie").CreateIncompleteKey();
                var keys = _db.Insert(new[] { entity });
                movie.Id = keys.First().Path.First().Id;
            }

        //public IUnityContainer CreateChildContainer()
        //{
        //    throw new NotImplementedException();
        //}

        // [END create]

        public void Delete(long id)
            {
                _db.Delete(id.ToKey());
            }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        // [START list]
        public MovieList List(int pageSize, string nextPageToken)
            {
                var query = new Query("Movie") { Limit = pageSize };
                if (!string.IsNullOrWhiteSpace(nextPageToken))
                    query.StartCursor = ByteString.FromBase64(nextPageToken);
                var results = _db.RunQuery(query);
                return new MovieList()
                {
                    Movies = results.Entities.Select(entity => entity.ToMovie()),
                    NextPageToken = results.Entities.Count == query.Limit ?
                        results.EndCursor.ToBase64() : null
                };
            }
            // [END list]

            public Movie Read(long id)
            {
                return _db.Lookup(id.ToKey())?.ToMovie();
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
                _db.Update(movie.ToEntity());
            }
        }


    
}
