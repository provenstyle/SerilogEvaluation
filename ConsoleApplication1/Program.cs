using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Configuration;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        private static ILogger logger;

        static void Main(string[] args)
        {
            var connectionString = "Data Source=.;Initial Catalog=SFAppLogs;Integrated Security=True;App=BenefitsAdmin";
            var table = "Logs";
            var columnOptions = new ColumnOptions
            {
                AdditionalDataColumns = new List<DataColumn>(),
                Level = {StoreAsEnum = false},
                TimeStamp = { ConvertToUtc = false},
                Properties = { },
                Store = new List<StandardColumn>()
            };

            logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.MSSqlServer(connectionString, table, autoCreateSqlTable: true)
                //.WriteTo.Console()
                .CreateLogger();

            Serilog.Debugging.SelfLog.Out = new StreamWriter("c:\\temp\\serilog.log");
            Serilog.Debugging.SelfLog.WriteLine("***From the selflog");
            logger.Debug("Foo");
      }
    }
}
