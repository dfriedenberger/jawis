using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;

namespace CoreShared.Service
{
    public interface IConfigReader
    {
        T ReadConfig<T>(string key) where T : class;
        T ReadStatus<T>(string key) where T : class;

        void WriteConfig<T>(string key,T data) where T : class;
        void WriteStatus<T>(string key, T data) where T : class;
        void AppendStatus<T>(string key, T obj) where T : class;
        IList<T> ReadStatusList<T>(string key) where T : class;

    }
}
