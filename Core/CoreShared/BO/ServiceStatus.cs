﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared.BO
{
    public class ServiceStatus
    {
        public DateTime TimeStamp { get; set; }
        public ServiceState State { get; set; }
    }
}
