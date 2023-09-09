using AventStack.ExtentReports.Listener.Entity;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentXReporter : IReporterConfigurable, IObserver<ReportEntity>
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ConcurrentBag<int> _record = new ConcurrentBag<int>();

        private CancellationTokenSource _cancellationTokenSource;

        public bool TransferDelta { get; set; } = true;

        public ExtentXReporter()
        {
        }

        public void LoadJSONConfig(string filePath)
        {
        }

        public void LoadXMLConfig(string filePath)
        {
        }

        public void LoadConfig(string filePath)
        {
            LoadXMLConfig(filePath);
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(ReportEntity value)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => TransferReportAsync(value.Report)).Wait();
            Task.Run(() => TransferTestsAsync(value.Report.Tests, _cancellationTokenSource.Token)).Wait();           
        }

        private async Task<HttpResponseMessage> TransferReportAsync(Report report)
        {
            StringContent content = GetContent(report);
            string r = JsonConvert.SerializeObject(report);

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:8080/api/reports", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error sending report: " + response.StatusCode);
                }
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private StringContent GetContent(IBaseEntity entity)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string json = JsonConvert.SerializeObject(entity, jsonSerializerSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<HttpResponseMessage[]> TransferTestsAsync(IList<Test> tests, CancellationToken token)
        {
            var tasks = tests.Where(x => !_record.Contains(x.Id)).Select(x => SendTestAsync(x, token));
            return await Task.WhenAll(tasks);
        }

        private async Task<HttpResponseMessage> SendTestAsync(Test test, CancellationToken token)
        {
            StringContent content = GetContent(test);

            try
            {
                token.ThrowIfCancellationRequested();

                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:8080/api/tests", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error sending test: " + response.StatusCode);
                }
                else
                {
                    _record.Add(test.Id);
                }

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
