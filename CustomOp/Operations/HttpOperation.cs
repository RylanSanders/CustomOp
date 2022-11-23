using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class HttpOperation : Operation
    {
        public HttpOperation(XElement config) : base(config)
        {
            
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            //string s = (string)Operation.parseVars(data.getString("HttpRequestURL"), data, null);
            Logger.log.Info(data.getString("HttpRequestURL"));
            string response = CallUrl(data.getString("HttpRequestURL")).Result;
            data.put("HttpResponse",response);
        }

        public static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }
    }
}
