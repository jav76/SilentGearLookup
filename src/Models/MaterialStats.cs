
using Newtonsoft.Json;

namespace SilentGearLookup.Models
{
    internal class MaterialStats
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

}
