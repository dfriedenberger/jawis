using CoreShared.BO;
using CoreShared.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreImpl.Service
{
    class ConfigReader : IConfigReader
    {
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
            var jsondata = File.ReadAllText(configFile);
            return JsonConvert.DeserializeObject<T>(jsondata);
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
                var lines = File.ReadAllLines(file);
                foreach(var line in lines)
                {
                    var obj = JsonConvert.DeserializeObject<T>(line);
                    lst.Add(obj);
                }
            }
            return lst;
        }
    }
}
