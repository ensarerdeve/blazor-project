using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ProtaTestTrack2.Model
{
    public class RootFeature
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string RootID { get; set; }
        
        [BsonElement("Features")]
        public List<Feature> Features { get; set; } = new List<Feature>();
        public string Discriminator { get; set; } = "RootFeature";
    }
    public class Feature
    {
        public Feature()
        {
            Cases = new List<Case>();
            ChildFeatures = new List<Feature>();
        }
        
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string FeatureID { get; set; }
        
        [BsonElement("Name")]
        public string Name { get; set; }
        
        [BsonElement("ParentFeatureID")]
        public string? ParentFeatureID { get; set; } = null;
        [BsonElement("Cases")]
        public List<Case> Cases { get; set; }
        
        [BsonElement("ChildFeatures")]
        public List<Feature> ChildFeatures { get; set; }
        public string Discriminator { get; set; } = "Feature";
    }
    public class Case
    {
        public Case()
        {
            ExcludedVersions = new List<string>();
            IncluedVersions = new List<string>();
            History = new List<CaseHistory>();
        }
        
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string CaseID { get; set; }
        public string Discriminator { get; set; } = "Case";

        [BsonElement("Name")]
        public string Name { get; set; }
        
        [BsonElement("ParentFeatureID")]
        public string ParentFeatureID { get; set; }
        
        [BsonElement("ExternalProjectLink")]
        public string ExternalProjectLink { get; set; }
        
        [BsonElement("LatestStatus")]
        public bool LatestStatus { get; set; }
        
        [BsonElement("ExcludedVersions")]
        public List<string> ExcludedVersions { get; set; }
        
        [BsonElement("IncluedVersions")]
        public List<string> IncluedVersions { get; set; }
        
        [BsonElement("History")]
        public List<CaseHistory> History { get; set; }
    }

    public class CaseHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string CaseHistoryID { get; set;}
        [BsonElement("ParentCaseID")]
        public string ParentCaseID { get; set; }
        public string Discriminator { get; set; } = "CaseHistory";

        
        [BsonElement("Date")]
        public DateTime Date { get; set; }
        
        [BsonElement("Notes")]
        public string Notes { get; set; }
        
        [BsonElement("Tester")]
        public string Tester { get; set; }
        
        [BsonElement("JiraNumber")]
        public string JiraNumber { get; set; }
        
        [BsonElement("Status")]
        public bool Status { get; set; }
    }
}

// public class AlphaViews
// {
//     [BsonElement("AlphaVersion")]
//     public string AlphaVersion { get; set; }
    
//     [BsonElement("SK")]
//     public string SK { get; set; } = "ROOT";
// }