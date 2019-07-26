using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace budgetif.Processor
{
    public class WebConnector
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string Get(string url, Dictionary<string, object> @params = null, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            var getData = string.Empty;
            if (@params != null)
            {
                foreach (string key in @params.Keys)
                {
                    getData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(@params[key].ToString()) + "&";
                }
            }

            if (!string.IsNullOrWhiteSpace(getData))
            {
                getData = getData.Substring(0, getData.Length - 1);
            }

            var getUrl = url + (!string.IsNullOrWhiteSpace(getData) ? ("?" + getData) : "");

            var httpRequest = WebRequest.Create(getUrl);
            logger.Debug($"GET {getUrl}");

            httpRequest.Credentials = CredentialCache.DefaultCredentials;
            httpRequest.Method = "GET";

            var response = httpRequest.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream, encoding);
            var result = reader.ReadToEnd();

            return result;
        }

        public string Post(string url, Dictionary<string, string> postParameters)
        {
            var request = WebRequest.Create(url);
            logger.Debug($"POST {url}");
            var postData = string.Empty;
            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]) + "&";
            }

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var result = reader.ReadToEnd();

            return result;
        }


        public string PostJson(string url, object json)
        {
            var request = WebRequest.Create(url);
            logger.Debug($"POST {url}");
            var postData = JsonConvert.SerializeObject(json);
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var result = reader.ReadToEnd();

            return result;
        }
    }
}
