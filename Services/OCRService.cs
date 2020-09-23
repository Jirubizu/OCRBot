using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json.Linq;
using OCRBot.Structs;

namespace OCRBot.Services
{
    public class OCRService
    {
        private readonly ConfigService config;
        private readonly HttpService http;
        private readonly TranslateService translate;

        public OCRService(ConfigService cs, HttpService hs, TranslateService ts)
        {
            config = cs;
            http = hs;
            translate = ts;
        }

        public async Task<string> Ocr(string imageUrl)
        {
            var res = await http.GetJsonAsync<OCRSpaceJson>(
                $"https://api.ocr.space/parse/imageurl?apikey={config.Config.OcrToken}&url={imageUrl}");
            
            if (res.IsErroredOnProcessing)
            {
                return $"Errored with the following code: {res.OCRExitCode}";
            }

            var str = new StringBuilder();
            foreach (var result in res.ParsedResults)
            {
                str.Append(result.ParsedText);
            }

            return ($"```{str}```");
        }

        public async Task<List<EmbedBuilder>> OcrTranslate(string imageUrl, string language)
        {
            var res = await http.GetJsonAsync<OCRSpaceJson>(
                $"https://api.ocr.space/parse/imageurl?apikey={config.Config.OcrToken}&url={imageUrl}&language={language.TrimEnd()}");
            
            

            if (res.IsErroredOnProcessing)
            {
                return null;
            }

            var str = new StringBuilder();
            foreach (var result in res.ParsedResults)
            {
                str.Append(result.ParsedText.Trim());
            }
            
            
            JArray json = await translate.GetJArray(EvalLang(language), "en", str.ToString());
            if (!json.HasValues || !json[0].HasValues || !json[0][0].HasValues) return null;
            
            var output = new StringBuilder();
            foreach (var line in json[0])
            {
                output.Append(line[0]);
            }

            var pages = new List<EmbedBuilder>
            {
                new EmbedBuilder().AddField("Original Text", str),
                new EmbedBuilder().AddField("Translated Text", output)
            };
            return pages;
        }

        private static string EvalLang(string language)
        {
            var dict = new Dictionary<string, string>
            {
                {"bul", "bg"},
                {"chs", "zh-CN"},
                {"cht", "zh-TW"},
                {"cze", "cs"},
                {"dut", "nl"},
                {"ger", "de"},
                {"jpn", "ja"},
                {"pol", "pl"},
                {"por", "pt"},
                {"spa", "es"},
                {"swe", "sv"},
                {"tur", "tr"},
            };

            return dict.TryGetValue(language, out var val) ? val : language.Substring(0, 2);
        }
    }
}