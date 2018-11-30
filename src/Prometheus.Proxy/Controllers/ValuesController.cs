using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prometheus.Proxy.Configuration;

namespace Prometheus.Proxy.Controllers
{
    [Route("metrics-text")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly string proxyTarget;

        public ValuesController(IOptions<ProxyConfiguration> target)
        {

                        this.proxyTarget = target.Value.ProxyTarget;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var ret = await httpClient.GetStringAsync(this.proxyTarget);

            return CleanMetricNames(ret);
        }

        private static string CleanMetricNames( string ret)
        {
            return ret
                .Replace("COUNTER", "counter", StringComparison.InvariantCulture)
                .Replace("GAUGE", "gauge", StringComparison.InvariantCulture)
                .Replace("SUMMARY", "summary", StringComparison.InvariantCulture)
                .Replace("UNTYPED", "untyped", StringComparison.InvariantCulture)
                .Replace("HISTOGRAM", "histogram", StringComparison.InvariantCulture);
        }
    }
}
