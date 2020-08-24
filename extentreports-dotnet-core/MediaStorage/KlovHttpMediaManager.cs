using AventStack.ExtentReports.Model;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace AventStack.ExtentReports.MediaStorage
{
    internal class KlovHttpMediaManager
    {
        private const string Route = "files/upload";

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
        public void StoreMedia(Media m)
        {
            if (string.IsNullOrEmpty(m.Path) || !string.IsNullOrEmpty(m.Base64String))
            {
                return;
            }

            if (!File.Exists(m.Path))
            {
                throw new IOException("The system cannot find the file specified " + m.Path);
            }

            var uri = new Uri(_host);
            var file = File.ReadAllBytes(m.Path);
            var fileName = m.Sequence + Path.GetExtension(m.Path);

            using (var reader = new StreamReader(m.Path))
            {
                using (var handler = new HttpClientHandler() { UseCookies = false })
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = uri;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                    using (var content = new MultipartFormDataContent(DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                    {
                        string logId = m.LogObjectId == null ? "" : m.LogObjectId.ToString();

                        var values = new[]
                        {
                            new KeyValuePair<string, string>("name", m.Sequence + Path.GetExtension(m.Path)),
                            new KeyValuePair<string, string>("id", m.ObjectId.ToString()),
                            new KeyValuePair<string, string>("reportId", m.ReportObjectId.ToString()),
                            new KeyValuePair<string, string>("testId", m.TestObjectId.ToString()),
                            new KeyValuePair<string, string>("mediaType", "img"),
                            new KeyValuePair<string, string>("logId", logId)
                        };

                        foreach (var keyValuePair in values)
                        {
                            content.Add(new StringContent(keyValuePair.Value), String.Format("\"{0}\"", keyValuePair.Key));
                        }

                        var imageContent = new ByteArrayContent(file);
                        content.Add(imageContent, "f", fileName);
                        
                        var result = client.PostAsync(_host + Route, content).Result;

                        if (result.StatusCode != HttpStatusCode.OK)
                        {
                            var fullResponseMessage = result.Content.ReadAsStringAsync().Result;
                            throw new IOException($"Unable to upload file to server: {m.Path}. Response code: {result.StatusCode}. Full response: {fullResponseMessage}");
                        }
                    }
                }
            }
        }
    }
}
