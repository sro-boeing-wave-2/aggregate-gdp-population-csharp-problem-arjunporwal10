using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AggregateGDPPopulation
{
    public class GDPPopulation
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }
    public class Class1
    {
        public async Task<string> ReadFile(string filePath)
        {
            string content = "";
            using (StreamReader reader = new StreamReader(filePath))
            {
                content = await reader.ReadToEndAsync();
            }
            return content;
        }
        public async Task WriteFile(Dictionary<String, GDPPopulation> dictionaryOfObjects)
        {
            string outputFilePath = Environment.CurrentDirectory + @"/output/output.json";
            if (!Directory.Exists(Environment.CurrentDirectory + @"/output"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"/output");
            }
            string JsonObject = JsonConvert.SerializeObject(dictionaryOfObjects);
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    await writer.WriteAsync(JsonObject);
                }  
        }
        public async Task AggregateGDPPopulation(string filePath)
        {
            string mapperFilePath = @"../../../../AggregateGDPPopulation/data/countryContinentJsonFile.json";
            var ReadTask1 = ReadFile(filePath);
            var Readtask2 = ReadFile(mapperFilePath);
            await Task.WhenAll(ReadTask1,Readtask2);
            var Mapper = JsonConvert.DeserializeObject<Dictionary<string, string>>(Readtask2.Result);
            string[] AllDataProcessed = ReadTask1.Result.Replace("\"", String.Empty).Trim().Split('\n');
            string[] headers = AllDataProcessed[0].Split(',');
            int indexOfPopulation = Array.IndexOf(headers, "Population (Millions) 2012");
            int indexOfGDP = Array.IndexOf(headers, "GDP Billions (USD) 2012");
            int indexOfCountries = Array.IndexOf(headers, "Country Name");
            Dictionary<string, GDPPopulation> objectDictonary = new Dictionary<string, GDPPopulation>();
            for (int i = 1; i < ReadTask1.Result.Length; i++)
            {
                try
                {
                    string[] row = AllDataProcessed[i].Split(',');
                    string countryName = row[indexOfCountries];
                    string nameOfContinent = Mapper[countryName];
                    float Population = float.Parse(row[indexOfPopulation]);
                    float Gdp = float.Parse(row[indexOfGDP]);
                    try
                    {
                        objectDictonary[nameOfContinent].GDP_2012 += Gdp;
                        objectDictonary[nameOfContinent].POPULATION_2012 += Population;
                    }
                    catch
                    {
                        GDPPopulation Object = new GDPPopulation() { GDP_2012 = Gdp, POPULATION_2012 = Population };
                        objectDictonary.Add(nameOfContinent, Object);
                    }
                }
                catch { }               
            }
            await WriteFile(objectDictonary);
        }
    }
}
