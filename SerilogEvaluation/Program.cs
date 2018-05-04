namespace SerilogEvaluation
{
    using System;
    using System.IO;
    using Castle.Facilities.Logging;
    using Castle.Services.Logging.SerilogIntegration;
    using Castle.Windsor;
    using Serilog;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using Serilog.Debugging;
    using Serilog.Sinks.MSSqlServer;

    class Program
    {
        private static Castle.Core.Logging.ILogger logger;

        static void Main(string[] args)
        {
            //Serilog Internal logging
            SelfLog.Enable(Console.Error);
            //SelfLog.Enable(new StreamWriter("c:\\temp\\serilog.log"));

            var columnOptions = new ColumnOptions();
            columnOptions.Level.ColumnName      = "LogLevel";
            columnOptions.Message.ColumnName    = "LogMessage";
            columnOptions.TimeStamp.ColumnName  = "LogTimeStamp";
            columnOptions.AdditionalDataColumns = new List<DataColumn> {
                new DataColumn("ApplicationName", typeof(string))
            };

            var config = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationName", Assembly.GetExecutingAssembly().GetName().Name)
                .WriteTo.MSSqlServer("Serilog", "Logs", columnOptions:columnOptions)
                .ReadFrom.AppSettings()
                .CreateLogger();

            var container = new WindsorContainer();
            container.AddFacility<LoggingFacility>(f => f.LogUsing(new SerilogFactory(config)));
            logger = container.Resolve<Castle.Core.Logging.ILogger>();

            config.Verbose("This is Verbose");
            logger.Debug("Foo");
            logger.DebugFormat("My message {@TestData}", new {Foo = "Foo data", Bar = "Bar value"});
            logger.DebugFormat("My object {@TestData}", new Fizz("asdf"));
            logger.DebugFormat("My object {}", new Fizz("asdf"));
            logger.Warn("This is a warning.");
            logger.Error("Testing errors", new ArgumentException("Test"));

            config.Dispose(); //Calling dispose here just to make sure serilog flushes the batch before we exit
        }
    }

    public class Fizz
    {
        public Fizz(string value)
        {
            Buzz = value;
        }
        public string Buzz { get; set; }
    }
}
