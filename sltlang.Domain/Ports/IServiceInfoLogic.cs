using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sltlang.Common.Common;

namespace sltlang.Domain.Ports
{
    public interface IServiceInfoLogic
    {
        Task<IDictionary<string, ServiceInfo>> GetServiceInfos();
    }
}
