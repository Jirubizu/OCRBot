using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using OCRBot.Utilities.Comparers;

namespace OCRBot.Services
{
    public class HelpService
    {
        private readonly CommandService commandService;
        public IEnumerable<CommandInfo> NoNameDuplicates;

        public HelpService(CommandService c)
        {
            commandService = c;
            Setup();
        }
        
        private void Setup()
        {
            NoNameDuplicates = commandService.Commands.Distinct(new CommandInfoComparer());
        }
    }
}