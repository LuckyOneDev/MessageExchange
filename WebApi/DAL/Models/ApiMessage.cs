using System.Text.Json.Serialization;

namespace WebApi.DAL.Models
{
    public class ApiMessage
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
