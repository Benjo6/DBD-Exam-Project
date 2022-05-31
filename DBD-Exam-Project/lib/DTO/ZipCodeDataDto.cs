
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lib.DTO
{
    public class ZipCodeDataDto
    {
        [JsonPropertyName("nr")]
        public string ZipCode { get; set; }
        [JsonPropertyName("navn")]
        public string CityName { get; set; }
        [JsonPropertyName("visueltcenter")]
        public List<double> Center { get; set; }
        [JsonPropertyName("bbox")]
        public List<double> BBox { get; set; }
    }
}