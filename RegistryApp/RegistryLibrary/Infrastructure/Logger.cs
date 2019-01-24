using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace RegistryLibrary.Infrastructure
{
    public static class Logger
    {
        public static string FullName { get; set; }

        private static string loggerFile = "Log.csv";
        static string foloderLocation = ConfigurationManager.AppSettings["folders"];

        private static string GetLogLocation
        {
            get { return foloderLocation + loggerFile; }
        }

        public static List<string> ReadAllLogLines()
        {

            if (!File.Exists(GetLogLocation))
            {               
                return new List<string>();
            }

            return File.ReadAllLines(GetLogLocation).ToList();
        }

        public static void DeleteLogFiles()
        {
            if (File.Exists(GetLogLocation))
            {
                File.Delete(GetLogLocation);
            }
        }

        public static List<LoggerModel> GetLoggers(List<string> lines)
        {
            if (lines.Count > 0)
            {
                List<LoggerModel> output = new List<LoggerModel>();

                foreach (string line in lines)
                {
                    string[] model = line.Split(',');
                    LoggerModel logger = new LoggerModel();
                    logger.LoggerName = model[0];
                    logger.Event = model[1];
                    logger.Date = model[2];
                    output.Add(logger);
                }
                return output;
            }
            else
            {
                return new List<LoggerModel>();
            }
        }

        private static void WriteLoggers(List<LoggerModel> loggers)
        {
            string path = GetLogLocation;
            List<string> lines = new List<string>();
            foreach (var log in loggers)
            {
                lines.Add($"{log.LoggerName},{log.Event},{log.Date}");
            }

            File.WriteAllLines(path, lines);
        }


        public static void WriteToFile(string loggerName, string logEvent)
        {
            LoggerModel logger = new LoggerModel()
            {
                LoggerName = loggerName,
                Event = logEvent,

            };

            List<LoggerModel> allLogers = GetLoggers(ReadAllLogLines());
            allLogers.Add(logger);
            WriteLoggers(allLogers);
        }
    }
}
