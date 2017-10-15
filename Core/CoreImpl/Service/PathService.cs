using CoreShared.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreImpl.Service
{
    class PathService : IPathService
    {
        public PathService()
        {
            var path = ConfigurationManager.AppSettings["ConfigPath"];
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //Log Path
            var logPath = Path.Combine(path, "log");
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);


            ConfigPath = Path.Combine(path, "config");
            if (!Directory.Exists(ConfigPath))
                Directory.CreateDirectory(ConfigPath);

            StatusPath = Path.Combine(path, "status");
            if (!Directory.Exists(StatusPath))
                Directory.CreateDirectory(StatusPath);
        }

        public string ConfigPath { get; private set; }
        public string StatusPath { get; private set; }
    }
}
