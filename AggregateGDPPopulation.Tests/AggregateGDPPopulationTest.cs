using System;
using Xunit;
using AggregateGDPPopulation;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Class1.AggregateGDPPopulation(@"D:\workspace\htmlplaygrnd\C#Assignment\aggregate-gdp-population-csharp-problem-arjunporwal10\AggregateGDPPopulation\data\datafile.csv");
            string actualFile = File.ReadAllText("../../../expected-output.json");
            string expectedFile = File.ReadAllText(Environment.CurrentDirectory + @"/output/output.json");
            Dictionary<string, GDPPopulation> actual = JsonConvert.DeserializeObject<Dictionary<string, GDPPopulation>>(actualFile);
            Dictionary<string, GDPPopulation> expected = JsonConvert.DeserializeObject<Dictionary<string, GDPPopulation>>(expectedFile);
            foreach (var key in actual.Keys)
            {
                if (expected.ContainsKey(key))
                {
                    Assert.Equal(actual[key].GDP_2012, expected[key].GDP_2012);
                    Assert.Equal(actual[key].POPULATION_2012, expected[key].POPULATION_2012);
                } else
                {
                    Assert.True(false);
                }
            }
        }
    }
}
