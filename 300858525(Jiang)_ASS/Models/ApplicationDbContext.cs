using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;

namespace _300858525_Jiang__ASS.Models
{
    public class ApplicationDbContext : DbContext
    {
        // [START_EXCLUDE]
        private static readonly string s_mySqlServerBaseName = "LocalMySqlServer";
        private static readonly string s_sqlServerBaseName = "LocalSqlServer";
        // [END_EXCLUDE]
        public DbSet<Movie> Movies { get; set; }
        // [END dbset]

        /// <summary>
        /// Pulls connection string from Web.config.
        /// </summary>
        public ApplicationDbContext() : base("name=" +
            ((UnityConfig.ChooseMovieStoreFromConfig() == MovieStoreFlag.MySql)
            ? s_mySqlServerBaseName : s_sqlServerBaseName))
        {
        }
    }
}
