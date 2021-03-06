﻿using CoreShared.BO;
using CoreShared.Service;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreImpl.Service
{
    public class ConfigReader : IConfigReader
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IPathService _pathService;

        public ConfigReader(IPathService pathService)
        {
            _pathService = pathService;
        }


        public T ReadConfig<T>(string key) where T : class
        {
            var configFile = Path.Combine(_pathService.ConfigPath, key + ".json");
            if (!File.Exists(configFile)) return null;
            var jsondata = File.ReadAllText(configFile);
            return JsonConvert.DeserializeObject<T>(jsondata);
        }

        public void WriteConfig<T>(string key, T data) where T : class
        {
            var txt = JsonConvert.SerializeObject(data,Formatting.Indented);
            var configFile = Path.Combine(_pathService.ConfigPath, key + ".json");
            File.WriteAllText(configFile,txt);
        }

        public T ReadStatus<T>(string key) where T : class
        {
            var configFile = Path.Combine(_pathService.StatusPath, key + ".json");
            if (!File.Exists(configFile)) return null;
            try
            {
                var jsondata = ReadAllText(configFile);
                return JsonConvert.DeserializeObject<T>(jsondata);
            } catch(Exception e)
            {
                _log.Error(e);
                return null;
            }
        }

        private string ReadAllText(string configFile)
        {
            using (var fs = new FileStream(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                return sr.ReadToEnd();
            }
        }

        public void WriteStatus<T>(string key, T data) where T : class
        {
            var txt = JsonConvert.SerializeObject(data, Formatting.Indented);
            var configFile = Path.Combine(_pathService.StatusPath, key + ".json");
            File.WriteAllText(configFile, txt);
        }

     
        public void AppendStatus<T>(string key, T obj) where T : class
        {
            var filename = string.Format("{0:yyyyMMdd}_{1}.txt", DateTime.Now, key);
            var configFile = Path.Combine(_pathService.StatusPath, filename);
            var txt = JsonConvert.SerializeObject(obj, Formatting.None);
            File.AppendAllText(configFile, txt + Environment.NewLine);

        }

        public IList<T> ReadStatusList<T>(string key) where T : class
        {
            var lst = new List<T>();
            var files = Directory.GetFiles(_pathService.StatusPath, "*_" + key + ".txt");
            foreach(var file in files)
            {
                var lines = File.ReadAllLines(file, Encoding.UTF8);
                foreach(var line in lines)
                {
                    var obj = JsonConvert.DeserializeObject<T>(line);
                    if(obj == null)
                    {
                        _log.Error("No Object found in line file:" + file + " line:" + line);
                        continue;
                    }
                    lst.Add(obj);
                }
            }
            return lst;
        }
    }
}
