using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DotNetBenchmark472vs6
{
    [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
    [MemoryDiagnoser]
    public class BenchmarkNet6Net472
    {
        public class WeatherForecast
        {
            public DateTimeOffset Date { get; set; }
            public int TemperatureCelsius { get; set; }
            public string Summary { get; set; }
            public string SummaryField;
            public IList<DateTimeOffset> DatesAvailable { get; set; }
            public Dictionary<string, HighLowTemps> TemperatureRanges { get; set; }
            public string[] SummaryWords { get; set; }
        }

        public class HighLowTemps
        {
            public int High { get; set; }
            public int Low { get; set; }
        }

        private readonly string _jsonString = @"{
              ""Date"": ""2019-08-01T00:00:00-07:00"",
              ""TemperatureCelsius"": 25,
              ""Summary"": ""Hot"",
              ""DatesAvailable"": [
                ""2019-08-01T00:00:00-07:00"",
                ""2019-08-02T00:00:00-07:00""
              ],
              ""TemperatureRanges"": {
                            ""Cold"": {
                                ""High"": 20,
                  ""Low"": -10
                            },
                ""Hot"": {
                                ""High"": 60,
                  ""Low"": 20
                }
                        },
              ""SummaryWords"": [
                ""Cool"",
                ""Windy"",
                ""Humid""
              ]
            }
        ";

        [Benchmark]
        public string SerializeJson()
        {
            string jsonString = JsonSerializer.Serialize(new WeatherForecast
            {
                Date = DateTime.Parse("2019-08-01"),
                TemperatureCelsius = 25,
                Summary = "Hot",
                SummaryField = "Hot",
                DatesAvailable = new List<DateTimeOffset> { DateTime.Parse("2019-08-01"), DateTime.Parse("2019-08-02") },
                TemperatureRanges = new Dictionary<string, HighLowTemps>
                {
                    ["Cold"] = new HighLowTemps { High = 20, Low = -10 },
                    ["Hot"] = new HighLowTemps { High = 60, Low = 20 },
                },
                SummaryWords = new[] { "Cool", "Windy", "Humid" },
            });

            return jsonString;
        }

        [Benchmark]
        public WeatherForecast DeserializeJson()
        {
            WeatherForecast result = JsonSerializer.Deserialize<WeatherForecast>(_jsonString);

            return result;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<BenchmarkNet6Net472>();
        }
    }
}