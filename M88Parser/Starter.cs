using M88Parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingWebTools
{
    internal class Starter
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Starter> _log;
        private readonly Parser _parser;

        public Starter(IConfiguration config, ILogger<Starter> log, Parser parser)
        {
            _config = config;
            _log = log;
            _parser = parser;
        }

        public async Task Run()
        {
            _log.LogInformation("Start Parsing www.m88.com");

            var sw = new Stopwatch();

            var all = sw.ElapsedMilliseconds;
            int count = 1;
            while(true)
            {
                try
                {
                    sw.Restart();
                    var data = await _parser.ParseLiveSoccerMatches();
                    var outputPath = _config.GetValue<string>("outPutFilePath") ?? "output.json";
                    File.WriteAllText(outputPath, JsonConvert.SerializeObject(data));
                    sw.Stop();
                    count++;
                    Console.WriteLine("Average = " + (all/(double)count)/1000);
                    all += sw.ElapsedMilliseconds;
                    Console.WriteLine("Success " + sw.Elapsed );
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }

    }
}
