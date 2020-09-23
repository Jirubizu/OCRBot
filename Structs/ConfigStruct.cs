using Newtonsoft.Json;

namespace OCRBot.Structs
{
    public class ConfigStruct
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("translate_base_url")]
        public string TranslateBaseUrl { get; set; }
        
        [JsonProperty("ocr_token")]
        public string OcrToken { get; set; }
        
        [JsonProperty("mongo_db_uri")]
        // ReSharper disable once InconsistentNaming
        public string MongoDBUri { get; set; }
        
        [JsonProperty("mongo_db_name")]
        // ReSharper disable once InconsistentNaming
        public string MongoDBName { get; set; }
        
    }
}