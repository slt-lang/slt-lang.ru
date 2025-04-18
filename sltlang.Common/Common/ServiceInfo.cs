using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Common.Common
{
    public class ServiceInfo
    {
        public DateTime DeployTime { get; set; }
        public DateTime StartTime { get; set; }
        public string Version { get; set; } = default!;

        public static ServiceInfo GetServiceInfo(Assembly assembly)
        {
            return new ServiceInfo()
            {
                DeployTime = File.GetLastWriteTime(assembly.Location),
                StartTime = Process.GetCurrentProcess().StartTime,
                Version = assembly.GetName().Version!.ToString(3),
            };
        }
    }
}
