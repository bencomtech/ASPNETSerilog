using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace ASPNETSerilog.API
{
    public class Program
    {
        private const string LogPath = "Logs";
        private const string SeqURI = "http://localhost:5341";
        private const string SeqAPIKey = "hmD3OLcgB8sRnLiW5THq";

        public static void Main(string[] args)
        {
            string logPath = string.Format(@"{0}\log.txt", LogPath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Hour)
                .WriteTo.Seq(SeqURI, apiKey: SeqAPIKey)
                .CreateLogger();

            try
            {
                Log.Information("Start");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Error");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
