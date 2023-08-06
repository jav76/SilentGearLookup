
namespace SilentGearLookup.Models
{
    internal class MaterialAvailability
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
}
