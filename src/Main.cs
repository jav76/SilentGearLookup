using Newtonsoft.Json;
using System.IO;
using System.Linq.Expressions;

namespace SilentGearLookup
{
    internal class main
    {
        private const string MATERIALS_DIR = "resources\\silentgear_materials";

        static void Main(string[] args)
        {

            List<string> materialsJSON = new List<string>();

            materialsJSON = ReadMaterialFiles();

            List<MaterialProperties> materials = new List<MaterialProperties>();

            foreach (string materialJSON in materialsJSON)
            {
                var deserializedJSON = JsonConvert.DeserializeObject<dynamic>(materialJSON);

                if (deserializedJSON != null)
                {

                    MaterialProperties newMaterial = new MaterialProperties
                    (
                        new MaterialAvailability(deserializedJSON.availability),
                        new MaterialName(deserializedJSON.name),
                        new MaterialStats(deserializedJSON.stats)
                    );

                    materials.Add(newMaterial);
                }

            }

            MaterialProperties highest;

            highest = GetHighestValue("armor", materials);


        }

        private static MaterialProperties GetHighestValue(string key, List<MaterialProperties> materials)
        {
            MaterialProperties highest = materials.Where(x => x.Stats != null && x.Stats.Main != null).First();

            foreach (MaterialProperties material in materials)
            {
                if (material.Stats != null && material.Stats.Main != null)
                {
                    if (material.Stats.Main.ContainsKey(key))
                    {
                        if (material.Stats.Main[key] > highest.Stats.Main[key])
                        {
                            highest = material;
                        }
                    }
                }
            }

            return highest;
        }

        private class MaterialProperties
        {

            public MaterialProperties(MaterialAvailability? availability, MaterialName? name, MaterialStats? stats)
            {
                Availability = availability;
                Name = name;
                Stats = stats;
            }

            public MaterialAvailability? Availability { get; set; }
            public MaterialName? Name { get; set; }
            public MaterialStats? Stats { get; set; }
        }

        private class MaterialAvailability
        {
            public MaterialAvailability(dynamic? dynamicJSON)
            {
                if (dynamicJSON != null)
                {
                    Tier = dynamicJSON.tier;
                    Categories = dynamicJSON.categories != null ? dynamicJSON.categories.ToObject<List<string>>() : null;
                    Salvagable = dynamicJSON.can_salvage;
                }

            }

            public string? Tier { get; set; }
            public List<string>? Categories { get; set; }
            public bool Salvagable { get; set; }
        }

        private class MaterialName
        {
            public MaterialName(dynamic? dynamicJSON)
            {
                if (dynamicJSON != null)
                {
                    TranslatedName = dynamicJSON.translate;
                }
            }
            public string? TranslatedName { get; set; }
        }

        private class MaterialStats
        {
            public MaterialStats(dynamic? dynamicJSON)
            {
                if (dynamicJSON != null)
                {
                    try
                    {
                        Rod = dynamicJSON.rod != null ? dynamicJSON.rod.ToObject<Dictionary<string, Dictionary<string, double>>>() : null;
                    }
                    catch (JsonReaderException ex)
                    {

                    }
                    catch (JsonSerializationException ex)
                    {

                    }


                    try
                    {
                        Tip = dynamicJSON.tip != null ? dynamicJSON.tip.ToObject<Dictionary<string, Dictionary<string, double>>>() : null;
                    }
                    catch (JsonReaderException ex)
                    {

                    }
                    catch (JsonSerializationException ex)
                    {

                    }

                    try
                    {
                        Main = dynamicJSON.main != null ? dynamicJSON.main.ToObject<Dictionary<string, double>>() : null;
                    }
                    catch (JsonReaderException ex)
                    {
                        try
                        {
                            MainMultipliers = dynamicJSON.main != null ? dynamicJSON.main.ToObject<Dictionary<string, Dictionary<string, double>>>() : null;
                        }
                        catch (Exception innerEx)
                        {
                            Console.WriteLine("idk man, we're fucked");
                        }
                    }
                }
            }
            public Dictionary<string, double>? Main { get; set; }
            public Dictionary<string, Dictionary<string, double>>? MainMultipliers { get; set; }
            public Dictionary<string, Dictionary<string, double>>? Rod { get; set; }
            public Dictionary<string, Dictionary<string, double>>? Tip { get; set; }

        }


        private static List<string> ReadMaterialFiles()
        {
            List<string> contents = new List<string>();
            string currentDir = Directory.GetCurrentDirectory();
            string materialsFullPath = Path.Combine(currentDir, MATERIALS_DIR);

            foreach (string file in Directory.GetFiles(materialsFullPath, "*.json", SearchOption.AllDirectories))
            {
                if (File.Exists(file))
                {
                    contents.Add(File.ReadAllText(file));
                }
            }

            return contents;

        }
    }
}