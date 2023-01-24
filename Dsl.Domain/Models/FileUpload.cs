using Newtonsoft.Json;

namespace Dsl.Domain.Models
{
    public class FileUpload
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("image")]
        public byte[] Image { get; set; }
    }
}