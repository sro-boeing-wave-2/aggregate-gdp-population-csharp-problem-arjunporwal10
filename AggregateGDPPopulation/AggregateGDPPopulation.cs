using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AggregateGDPPopulation
{
    public class GDPPopulation
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }
    public class Class1
    {
        public static void AggregateGDPPopulation(string FilePath)
        {

            string[] AllData = File.ReadAllLines(FilePath);
            StreamReader r = new StreamReader(@"../../../../AggregateGDPPopulation/data/countryContinentJsonFile.json");
            var json = r.ReadToEnd();
            r.Close();
            var Mapper = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            string[] headers = AllData[0].Split(',');
            int indexOfPopulation = Array.IndexOf(headers, "\"Population (Millions) 2012\"");
            int indexOfGDP = Array.IndexOf(headers, "\"GDP Billions (USD) 2012\"");
            int indexOfCountries = Array.IndexOf(headers, "\"Country Name\"");
            Dictionary<string, GDPPopulation> objectDictonary = new Dictionary<string, GDPPopulation>();
            for (int i = 1; i < AllData.Length; i++)
            {
                try
                {
                    string[] row = AllData[i].Split(',');
                    string countryName = row[indexOfCountries].Trim('"');
                    string nameOfContinent = Mapper[countryName];
                    float Population = float.Parse(row[indexOfPopulation].Trim('"'));
                    float Gdp = float.Parse(row[indexOfGDP].Trim('"'));
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
            var finalObject = JsonConvert.SerializeObject(objectDictonary);
            string outputFilePath = Environment.CurrentDirectory + @"/output/output.json";
            Console.WriteLine(outputFilePath);
            if (!Directory.Exists(Environment.CurrentDirectory + @"/output"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"/output");
            }
            StreamWriter outputdata = new StreamWriter(outputFilePath);
            outputdata.WriteLine(finalObject);
            outputdata.Close();
            
            //Console.ReadLine();
        }
    }
}
