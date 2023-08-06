using Newtonsoft.Json;
using System.IO;
using System.Linq.Expressions;
using SilentGearLookup;
using SilentGearLookup.Models;
using SilentGearLookup.Data;

namespace SilentGearLookup
{
    internal class main
    {
        private const string MATERIALS_DIR = "resources\\silentgear_materials";

        static async Task Main(string[] args)
        {

            List<string> materialsJSON = new List<string>();


            await MaterialsDataRepo.UpdateMaterialsFiles();

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

        private static List<string> ReadMaterialFiles()
        {
            List<string> contents = new List<string>();
            string currentDir = Directory.GetCurrentDirectory();
            string materialsFullPath = Path.Combine(currentDir, MaterialsDataRepo.LOCAL_MATERIALS_PATH);

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