namespace Aero.Azure.ArmTemplate
{
    public class AzureRegion
    {
        public AzureRegion(string name, string azureKey, string abbreviation)
        {
            Name = name;
            AzureKey = azureKey;
            Abbreviation = abbreviation;
            AbbreviationLower = Abbreviation.ToLowerInvariant();
        }

        public string Name { get; }

        public string AzureKey { get; }

        public string Abbreviation { get; }

        public string AbbreviationLower { get; }

        public AzureRegion RegionPair { get; set; }
    }
}
