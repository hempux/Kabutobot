using Microsoft.Extensions.Configuration;
using net.hempux.kabuto.Utilities;
using System.IO;

namespace net.hempux.kabuto.Options
{
    public static class AppOptions
    {
        private static IConfiguration configuration;
        public static void Initialize(IConfiguration Configuration)
        {
            configuration = Configuration;

            // Server listen adress (localhost:5000 inside docker)
            ListenAddress = Utils.InDocker ?
                "http://0.0.0.0:80" :
                configuration.GetSection("applicationUrl").Value ?? "http://localhost:3978";

            // Sqlite filename/path
            string filename = configuration.GetSection("SqliteDatabase").Value ?? "kabuto.sqlite";
            SqliteDatabase = Utils.InDocker ?
                string.Concat("/app/data/", filename) :
                string.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, filename);

        }

        public static string SqliteDatabase { get; private set; }
        public static string ListenAddress { get; private set; }
    }
}
