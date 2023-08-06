
namespace SilentGearLookup.Models
{
    internal class MaterialName
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

}
