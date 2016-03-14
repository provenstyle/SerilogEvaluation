using System;
using System.IO;
using Castle.Facilities.Logging;
using Castle.Services.Logging.SerilogIntegration;
using Castle.Windsor;
using Serilog;

namespace SerilogEvaluation
{
    class Program
    {
        private static Castle.Core.Logging.ILogger logger;

        static void Main(string[] args)
        {
            var config = new LoggerConfiguration()
                .ReadFrom.AppSettings();

            var container = new WindsorContainer();
            container.AddFacility<LoggingFacility>(f => f.LogUsing(new SerilogFactory(config)));
            logger = container.Resolve<Castle.Core.Logging.ILogger>(); 

            Serilog.Debugging.SelfLog.Out = new StreamWriter("c:\\temp\\serilog.log");
            Serilog.Debugging.SelfLog.WriteLine("***From the selflog");

            logger.Debug("Foo");
            logger.DebugFormat("My message {@TestData}", new {Foo = "Foo data", Bar = "Bar value"});
            logger.DebugFormat("My object {@TestData}", new Fizz("asdf"));
            logger.DebugFormat("My object {}", new Fizz("asdf"));
            logger.Error("Testing errors", new ArgumentException("Test"));
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
