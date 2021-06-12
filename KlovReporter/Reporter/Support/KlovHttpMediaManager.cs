using AventStack.ExtentReports.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace AventStack.ExtentReports.Reporter.Support
{
    internal class KlovHttpMediaManager
    {
        private const string Route = "api/files";

        private string _host;

        public void Init(string host)
        {
            if (!host.Substring(host.Length - 1).Equals("/"))
            {
                host += "/";
            }
            _host = host;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StoreMedia(ScreenCapture m)
        {
            var path = string.IsNullOrEmpty(m.ResolvedPath) ? m.Path : m.ResolvedPath;

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!File.Exists(path))
            {
                throw new IOException("The system cannot find the file specified " + m.Path);
            }

            var uri = new Uri(_host);

            var reader = new StreamReader(path);
            var handler = new HttpClientHandler() { UseCookies = false };

            if (_host.ToLower().StartsWith("https"))
            {
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            }

            var client = new HttpClient(handler)
            {
                BaseAddress = uri
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            var content = new MultipartFormDataContent(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            var logId = m.Info.ContainsKey("LogObjectId") ? new ObjectId(m.Info["LogObjectId"].ToString()).ToString() : null;

            var values = new[]
            {
                new KeyValuePair<string, string>("name", m.Id.ToString() + Path.GetExtension(m.Path)),
                new KeyValuePair<string, string>("id", new ObjectId(m.Info["ObjectId"].ToString()).ToString()),
                new KeyValuePair<string, string>("reportId", new ObjectId(m.Info["ReportObjectId"].ToString()).ToString()),
                new KeyValuePair<string, string>("testId", new ObjectId(m.Info["TestObjectId"].ToString()).ToString())
            }.ToList();

            if (logId != null)
            {
                values.Add(new KeyValuePair<string, string>("logId", logId));
            }

            foreach (var keyValuePair in values)
            {
                content.Add(new StringContent(keyValuePair.Value), string.Format("\"{0}\"", keyValuePair.Key));
            }

            var file = File.ReadAllBytes(path);
            var fileName = m.Id.ToString() + Path.GetExtension(path);
            var imageContent = new ByteArrayContent(file);
            content.Add(imageContent, "f", fileName);

            var result = client.PostAsync(_host + Route, content).Result;

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new IOException("Unable to upload file to server: " + m.Path);
            }
        }
    }
}