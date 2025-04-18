using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sltlang.Common.Common;
using sltlang.Domain.Ports;

namespace sltlang.Domain.Logic
{
    public class ServiceInfoLogic(Config config, IHttpService httpService) : IServiceInfoLogic
    {
        public async Task<IDictionary<string, ServiceInfo>> GetServiceInfos()
        {
            var ret = new ConcurrentDictionary<string, ServiceInfo>(config.PeerServices.Keys.ToDictionary(x => x, x => default(ServiceInfo))!);

            var requests = config.PeerServices.Keys.Select<string, Func<Task>>(x => async () =>
                {
                    try
                    {
                        var (success, response) = await httpService.JsonRequest<object, ServiceInfo>(x, "serviceinfo", [], HttpMethod.Get, null, TimeSpan.FromSeconds(1));
                        if (success)
                        {
                            ret[x] = response!;
                        }
                    }
                    catch { }
                }).Select(x => x()).ToArray();

            await Task.WhenAll(requests);

            return ret;
        }
    }
}
