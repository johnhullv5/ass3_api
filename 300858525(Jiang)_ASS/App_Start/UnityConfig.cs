using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _300858525_Jiang__ASS.Models;
using _300858525_Jiang__ASS.Services;
using Microsoft.Practices.Unity;

using System.Configuration;
using System.Data.Entity;
using System.Runtime.Serialization;
using Unity;

namespace _300858525_Jiang__ASS
{

    public class ConfigurationException : Exception, ISerializable
    {
        public ConfigurationException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// We can store the book data in different places.  This flag tells us where we to store
    /// the book data.
    /// </summary>
    public enum MovieStoreFlag
    {
        MySql,
        SqlServer,
        Datastore
    }
    public class UnityConfig
    {
        public static string GetConfigVariable(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (value == null)
                throw new ConfigurationException($"You must set the configuration variable {key}.");
            return value;
        }

        public static string ProjectId => GetConfigVariable("_300858525_Jiang__ASS:ProjectId");

        public static MovieStoreFlag ChooseMovieStoreFromConfig()
        {
            string movieStore = GetConfigVariable("_300858525_Jiang__ASS:BookStore")?.ToLower();
            switch (movieStore)
            {
                case "datastore":
                    return MovieStoreFlag.Datastore;

                case "mysql":
                    DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
                    return MovieStoreFlag.MySql;

                case "sqlserver":
                    return MovieStoreFlag.SqlServer;

                default:
                    throw new ConfigurationException(
                         "Set the configuration variable GoogleCloudSamples:BookStore " +
                         "to datastore, mysql or sqlserver.");
            }
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        public static void RegisterTypes(Unity.UnityContainer container)
        {
            switch (ChooseMovieStoreFromConfig())
            {
                case MovieStoreFlag.Datastore:
                    container.RegisterInstance(
                        new DatastoreMovieStore(ProjectId));
                    break;

                //case MovieStoreFlag.MySql:
                //    container.RegisterType<IMovieStore, DbMovieStore>();
                //    break;

                //case MovieStoreFlag.SqlServer:
                //    container.RegisterType<IMovieStore, DbMovieStore>();
                //    break;
            }

            container.RegisterInstance<ImageUploader>(
                new ImageUploader(
                  GetConfigVariable("GoogleCloudSamples:BucketName")
                )
            );
        }
    }
}
