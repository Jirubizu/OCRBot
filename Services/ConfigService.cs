using System.IO;
using Newtonsoft.Json;
using OCRBot.Structs;

namespace OCRBot.Services
{
    public class ConfigService
    {
        public ConfigStruct Config { get; }

        public ConfigService(string path)
        {
            Config = JsonConvert.DeserializeObject<ConfigStruct>(File.ReadAllText(path));
        }
    }
}