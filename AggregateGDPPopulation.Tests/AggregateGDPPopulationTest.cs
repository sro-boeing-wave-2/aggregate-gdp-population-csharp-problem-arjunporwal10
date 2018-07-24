using System;
using Xunit;
using AggregateGDPPopulation;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task IsEqualExpectedAndActualJson()
        {
            Aggregator myClassObject = new Aggregator(@"../../../../AggregateGDPPopulation/data/datafile.csv");
            Task aggregatedDataAsTask = myClassObject.Aggregate();
            string expectedFile = File.ReadAllText(Environment.CurrentDirectory + @"/output/output.json");

            await aggregatedDataAsTask;
            string actualFile = File.ReadAllText("../../../expected-output.json");
            Dictionary<string, GDPPopulation> actual = JsonConvert.DeserializeObject<Dictionary<string, GDPPopulation>>(actualFile);
            Dictionary<string, GDPPopulation> expected = JsonConvert.DeserializeObject<Dictionary<string, GDPPopulation>>(expectedFile);
            foreach (var key in actual.Keys)
            {
                if (expected.ContainsKey(key))
                {
                    Assert.Equal(actual[key].GDP_2012, expected[key].GDP_2012);
                    Assert.Equal(actual[key].POPULATION_2012, expected[key].POPULATION_2012);
                }
                else
                {
                    Assert.True(false);
                }
            }
        }
    }
}
