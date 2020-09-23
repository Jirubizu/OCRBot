using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using OCRBot.Services;
using HService = OCRBot.Services.HelpService;
using OCRBot.Utilities;
using CService = Discord.Commands.CommandService;

namespace OCRBot.Modules
{
    public class HelpModule : ModuleBase<ShardedCommandContext>
    {
        public CService CommandService { get; set; }

        public DatabaseService Database { get; set; }
        public HService HelpService { get; set; }

        [Command("help"), Alias("h"), Summary("Display the help window")]
        public async Task Help()
        {
            var builder = new StringBuilder();
            builder.Append("```cs\n");
            foreach (var command in HelpService.NoNameDuplicates)
            {
                builder.Append(
                    $"{Database.LoadRecordsByGuildId(Context.Guild.Id).Result.Prefix}{command.Name.ToLowerInvariant()} {"//".PadLeft(12 - command.Name.Length)} {command.Summary} \n");
            }

            builder.Append("```");
            var embed = new EmbedBuilder()
            {
                Description = builder.ToString(),
                Color = Color.Gold
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Command("help")]
        public async Task Help(string module)
        {
            var commands =
                CommandService.Commands.Select(c => c)
                    .Where(c => c.Name.ToLowerInvariant().Equals(module.ToLowerInvariant()));
            var builder = new StringBuilder("```cs\n");
            foreach (var command in commands)
            {
                if (command.Summary != null)
                {
                    builder.AppendLine($"// {command.Summary}");
                }

                if (command.Aliases.Count > 1)
                {
                    builder.AppendLine($"// Aliases: {string.Join(", ", command.Aliases.Skip(1))}");
                }

                if (command.Remarks != null)
                {
                    builder.AppendLine($"/*\n{command.Remarks}\n*/");
                }

                var paras = new StringBuilder();
                foreach (var commandParameter in command.Parameters)
                {
                    paras.Append($"[{StringUtilities.TypeFormat(commandParameter.Type.Name)} {commandParameter.Name}]");
                }

                builder.AppendLine(
                    $"{Database.LoadRecordsByGuildId(Context.Guild.Id).Result.Prefix}{command.Name.ToLowerInvariant()} {paras}\n");
            }

            builder.Append("```");

            await ReplyAsync(builder.ToString());
        }
    }
}