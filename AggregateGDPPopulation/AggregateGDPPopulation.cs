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

    public static class FileUtility
    {
        public static string ReadFileSync(string filePath)
        {
            return "";
        }

        public static async Task<string> ReadFileAsync(string filePath)
        {
            string content = "";
            using (StreamReader reader = new StreamReader(filePath))
            {
                content = await reader.ReadToEndAsync();
            }
            return content;
        }

        public static async Task WriteFileAsync(string writingData)
        {
            string outputFilePath = Environment.CurrentDirectory + @"/output/output.json";
            if (!Directory.Exists(Environment.CurrentDirectory + @"/output"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"/output");
            }
           
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                await writer.WriteAsync(writingData);
            }
        }

        public static void WriteFile(string filePath)
        {

        }
    }
    // AggregatedGDPPopulation expectedAggregatedData = new AggregatedGDPPopulation();
    // expectedAggregatedData.DeserializeFromJSON('expected-output.json');
    // expectedAggregatedData.AggregatedData

    // AggregatedGDPPopulation actualAggregatedData;

    public class AggregateGDPPopulation
    {
        private Dictionary<string, GDPPopulation> aggregatedData;

        public Dictionary<string, GDPPopulation> AggregatedData
        {
            get
            {
                return aggregatedData;
            }

            private set { }
        }

        public AggregateGDPPopulation()
        {
            aggregatedData = new Dictionary<string, GDPPopulation>();
        }

        public void AddOrUpdateData(float Population, float Gdp, string nameOfContinent)
        {
            if (AggregatedData.ContainsKey(nameOfContinent))
            {
                AggregatedData[nameOfContinent].GDP_2012 += Gdp;
                AggregatedData[nameOfContinent].POPULATION_2012 += Population;  
            }
            else
            {
                GDPPopulation Object = new GDPPopulation() { GDP_2012 = Gdp, POPULATION_2012 = Population };
                AggregatedData.Add(nameOfContinent, Object);  
            }
        }

        public string SerializeToJSON(Dictionary<String, GDPPopulation> dictionaryOfObjects)
        {
            return JsonConvert.SerializeObject(dictionaryOfObjects); 
        }

        public Dictionary<string, GDPPopulation> DeserializeFromJSON(string dataToDeserialize)
        {
            // Deserialize it and add some data to _aggregateData;
            return JsonConvert.DeserializeObject<Dictionary<string, GDPPopulation>>(dataToDeserialize);

        }
        public Dictionary<string, string> MapperDeserializeFromJSON(string dataToDeserialize)
        {
            // Deserialize it and add some data to _aggregateData;
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(dataToDeserialize);

        }

        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj);
        //}
    }
    

    public class Aggregator
    {
        public AggregateGDPPopulation agdp;
        public string dataFilePath;

        public Aggregator(string filePath)
        {
            agdp = new AggregateGDPPopulation();          
            dataFilePath = filePath; 
        }
        public async Task Aggregate()
        {
            // ReadFile
            
            string mapperFilePath = @"../../../../AggregateGDPPopulation/data/countryContinentJsonFile.json";
            var ReadCsvDataFileTask = FileUtility.ReadFileAsync(dataFilePath);
            var ReadMapperDataFileTask = FileUtility.ReadFileAsync(mapperFilePath);
            Dictionary<string, string> Mapper = agdp.MapperDeserializeFromJSON(ReadMapperDataFileTask.Result);
            string[] AllDataProcessed = ReadCsvDataFileTask.Result.Replace("\"", String.Empty).Trim().Split('\n');
            string[] headers = AllDataProcessed[0].Split(',');
            int indexOfPopulation = Array.IndexOf(headers, "Population (Millions) 2012");
            int indexOfGDP = Array.IndexOf(headers, "GDP Billions (USD) 2012");
            int indexOfCountries = Array.IndexOf(headers, "Country Name");
            // OnEachLine call agdp.AddOrUpdateContinentData();
            
            for (int i = 1; i < ReadCsvDataFileTask.Result.Length; i++)
            {
                
                try
                {   
                    string[] row = AllDataProcessed[i].Split(',');
                    string countryName = row[indexOfCountries];
                    string nameOfContinent = Mapper[countryName];
                    float Population = float.Parse(row[indexOfPopulation]);
                    float Gdp = float.Parse(row[indexOfGDP]);
                    agdp.AddOrUpdateData(Population,Gdp,nameOfContinent); 
                }
                catch { }
            }
            await FileUtility.WriteFileAsync(agdp.SerializeToJSON(agdp.AggregatedData));
        }
    }

}
