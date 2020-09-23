using System;
using System.Threading.Tasks;

namespace OCRBot
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            await new OCRBot().SetupAsync();
        }
    }
}
