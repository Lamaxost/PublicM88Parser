using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParsingWebTools.RequestOptions;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParsingWebTools
{
    public class WebScrapper : IDisposable
    {
        private HttpClient defaultClient = default!;
        private int proxyCounter = -1;
        private readonly List<HttpClient> proxiedClients = new List<HttpClient>();
        private readonly IConfiguration _config;
        private bool _disposedValue;

        public WebScrapper(IConfiguration config)
        {
            _config = config;
            BuildClientsAndProxys();
        }

        private void BuildClientsAndProxys()
        {
            if (_config.GetRequiredSection("Scrapper").GetValue<bool>("UseProxy"))
            {
                var proxyPath = _config.GetRequiredSection("Scrapper").GetValue<string>("proxyPath") ?? "proxy.txt";
                var proxys = File.ReadAllLines(proxyPath).ToList();

                foreach (var proxy in proxys)
                {
                    var container = new CookieContainer();

                    var handler = new HttpClientHandler()
                    {
                        UseProxy = true,
                        UseCookies = true,
                        CookieContainer = container,
                        Proxy = new WebProxy()
                        {
                            Address = new Uri("http://" + proxy.Split("@")[1]),
                            Credentials = new NetworkCredential()
                            {
                                UserName = proxy.Split("@")[0].Split(":")[0],
                                Password = proxy.Split("@")[0].Split(":")[1]
                            }
                        }
                    };

                    var client = new HttpClient(handler);
                    client.DefaultRequestHeaders.Add("Host", _config.GetRequiredSection("Scrapper").GetValue<string>("Host"));
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
                    client.DefaultRequestHeaders.Add("User-Agent", _config.GetRequiredSection("Scrapper").GetValue<string>("User-Agent"));
                    client.DefaultRequestHeaders.Add("Accept", "*/*");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    proxiedClients.Add(client);
                }

            }
            var defaultClientContainer = new CookieContainer();
            var defaultClienthandler = new HttpClientHandler()
            {
                UseProxy = true,
                CookieContainer = defaultClientContainer
            };
            defaultClient = new HttpClient(defaultClienthandler);
            defaultClient.DefaultRequestHeaders.Add("Host", _config.GetRequiredSection("Scrapper").GetValue<string>("Host"));
            defaultClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            defaultClient.DefaultRequestHeaders.Add("User-Agent", _config.GetRequiredSection("Scrapper").GetValue<string>("User-Agent"));
            defaultClient.DefaultRequestHeaders.Add("Accept", "*/*");
            defaultClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

        public HttpClient GetClient(bool useProxy = true)
        {
            if (!useProxy)
            {
                return defaultClient;
            }
            lock (proxiedClients)
            {
                if (proxiedClients.Count == 0)
                {
                    return defaultClient;
                }
                proxyCounter = (proxyCounter + 1) % proxiedClients.Count;
                return proxiedClients[proxyCounter];
            }
        }
        public async Task<string> Get(string url, GetOptions? options = null, int attemptCount = 10)
        {
            attemptCount--;
            try
            {
                Dictionary<string, string>? additionalHeaders = options?.AdditionalHeaders;
                if (additionalHeaders == null)
                {
                    additionalHeaders = new Dictionary<string, string>();
                }
                HttpClient? client = options?.Client;
                if (client == null)
                {
                    client = GetClient(options?.useProxy ?? true);
                }

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                };
                foreach (var pair in additionalHeaders)
                {
                    request.Headers.Add(pair.Key, pair.Value);
                }

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                return Unzip(response.Content.ReadAsByteArrayAsync().Result);
            }
            catch
            {
                Task.Delay(100).Wait();
                if (attemptCount <=0)
                {
                    throw;
                }
                return await Get(url, options);
            }

        }

        public async Task<string> Post(string url, PostOptions? options = null, int attemptCount = 10)
        {

            attemptCount--;
            try
            {
                Dictionary<string, string>? additionalHeaders = options?.AdditionalHeaders;
                if (additionalHeaders == null)
                {
                    additionalHeaders = new Dictionary<string, string>();
                }
                HttpClient? client = options?.Client;
                if (client == null)
                {
                    client = GetClient(options?.useProxy ?? true);
                }

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                };
                foreach (var pair in additionalHeaders)
                {
                    request.Headers.TryAddWithoutValidation(pair.Key, pair.Value);
                }
                request.Content = options?.Content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return Unzip(response.Content.ReadAsByteArrayAsync().Result);
            }
            catch
            {
                Task.Delay(100).Wait();
                if (attemptCount <=0)
                {
                    throw;
                }
                return await Post(url, options,attemptCount);
            }

        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
        private static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                msi.Seek(0, SeekOrigin.Begin);
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var client in proxiedClients)
                    {
                        client.Dispose();
                    }
                    defaultClient.Dispose();
                }
                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
