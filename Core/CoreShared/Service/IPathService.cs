using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared.Service
{
    public interface IPathService
    {
        string ConfigPath { get;  }
        string StatusPath { get; }
    }
}
