using Newtonsoft.Json;

namespace OCRBot.Structs
{
    public class TextOverlayJson
    {
        [JsonProperty("Lines")]
        public string[] Lines { get; set; }

        [JsonProperty("HasOverlay")]
        public bool HasOverlay { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }

    public class ParsedResultJson
    {
        [JsonProperty("TextOverlay")]
        public TextOverlayJson TextOverlay { get; set; }

        [JsonProperty("TextOrientation")]
        public string TextOrientation { get; set; }

        [JsonProperty("FileParseExitCode")]
        public int FileParseExitCode { get; set; }

        [JsonProperty("ParsedText")]
        public string ParsedText { get; set; }

        [JsonProperty("ErrorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("ErrorDetails")]
        public string ErrorDetails { get; set; }
    }

    public class OCRSpaceJson
    {
        [JsonProperty("ParsedResults")]
        public ParsedResultJson[] ParsedResults { get; set; }

        [JsonProperty("OCRExitCode")]
        public int OCRExitCode { get; set; }

        [JsonProperty("IsErroredOnProcessing")]
        public bool IsErroredOnProcessing { get; set; }

        [JsonProperty("ProcessingTimeInMilliseconds")]
        public int ProcessingTimeInMilliseconds { get; set; }

        [JsonProperty("SearchablePDFURL")]
        public string SearchablePDFURL { get; set; }
    }
}