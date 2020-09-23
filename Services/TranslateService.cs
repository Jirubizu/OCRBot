using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json.Linq;

namespace OCRBot.Services
{
    public class TranslateService
    {
        private readonly HttpService http;
        private readonly ConfigService config;
        
        public TranslateService(HttpService hs, ConfigService cs)
        {
            http = hs;
            config = cs;
        }

        public async Task<JArray> GetJArray(string from, string to, string text)
        {
            from = Uri.EscapeDataString(from);
            to = Uri.EscapeDataString(to);
            text = Uri.EscapeDataString(text);
            
            var res = await http.GetJArrayAsync(
                $"{config.Config.TranslateBaseUrl}sl={from}&tl={to}&dt=t&q={text}");
            Console.WriteLine($"{config.Config.TranslateBaseUrl}sl={from}&tl={to}&dt=t&q={text}");
            return res;
        }

        public async Task<EmbedBuilder> GetTranslatedEmbed(string from, string to, string text)
        {
            JArray json = await GetJArray(from, to, text);

            // ReSharper disable once PossibleNullReferenceException
            if (json.HasValues && json[0].HasValues && json[0][0].HasValues)
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Color = Color.Teal,
                    Title = "Your Translation",
                    Fields = new List<EmbedFieldBuilder>
                    {
                        new EmbedFieldBuilder {Name = "Original Text", Value = text},
                        new EmbedFieldBuilder
                            {Name = "Translated text", Value = json[0][0][0] ?? ""}
                    },
                    Footer = new EmbedFooterBuilder {Text = "Powered by Google"},
                };
                return embed;
            }

            return new EmbedBuilder();
        }
    }
}