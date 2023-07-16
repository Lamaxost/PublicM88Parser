using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingWebTools
{
    public class PagesFactory : IDisposable
    {
        private readonly IConfiguration _config;
        private readonly List<string> proxys = new List<string>();
        private readonly List<Browser> browsers = new List<Browser>();
        private readonly bool UseProxy;
        private bool _disposedValue;
        private int proxyCounter = 0;

        public PagesFactory(IConfiguration config)
        {
            _config = config;

            UseProxy = _config.GetRequiredSection("Pages").GetValue<bool>("UseProxy");

            if (UseProxy)
            {
                var proxyPath = _config.GetRequiredSection("Pages").GetValue<string>("proxyPath")??"proxys.txt";
                if (string.IsNullOrEmpty(proxyPath))
                {
                    this.proxys = new List<string>();
                    UseProxy = false;
                }
                this.proxys = File.ReadAllLines(proxyPath).ToList();
                if (this.proxys.Count == 0)
                {
                    UseProxy = false;
                }
                browsers = new List<Browser>();
            }
        }
        public async Task<List<Page>> CreatePagesAsync(int count)
        {
            List<Page> pages = new List<Page>();

            for (int i = 0; i < count; i++)
            {

                string[] args = new string[] { };

                string? proxy = null;
                proxyCounter = (proxyCounter + 1) % proxys.Count;
                proxy = proxys[proxyCounter];

                if (UseProxy)
                {

                    args = new string[] { $"--proxy-server={proxy.Split("@")[1]}" };
                }
                var extra = new PuppeteerExtraSharp.PuppeteerExtra();
                var browser = await extra.LaunchAsync(new PuppeteerSharp.LaunchOptions()
                {
                    Headless = false,
                    ExecutablePath = _config.GetSection("ChromePath").Value,
                    Args = args
                });

                var page = await browser.NewPageAsync();

                if (UseProxy)
                {
                    var browserCredentials = new Credentials() { Username = proxy.Split("@")[0].Split(":")[0], Password = proxy.Split("@")[0].Split(":")[1] };
                    await page.AuthenticateAsync(browserCredentials);
                }

                var cookies = JsonConvert.DeserializeObject<CookieParam[]>(File.ReadAllText("cookies.txt"));
                await page.SetCookieAsync(cookies);

                pages.Add(page);
                browsers.Add(browser);
            }
            return pages;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var browser in browsers)
                    {
                        browser.Dispose();
                    }
                }
                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
