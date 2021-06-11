using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using OCRBot.Services;

namespace OCRBot.Modules
{
    public class UtilModule : ModuleBase<ShardedCommandContext>
    {
        public DatabaseService Database { get; set; }
        public TranslateService Translate { get; set; }

        private static string Uptime
        {
            get
            {
                var time = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);
                return new StringBuilder()
                    .Append((time.Days != 0 ? time.Days : 00) + "d:")
                    .Append((time.Hours != 0 ? time.Hours : 00) + "h:")
                    .Append((time.Minutes != 0 ? time.Minutes : 00) + "m:")
                    .Append((time.Seconds != 0 ? time.Seconds : 00) + "s").ToString();
            }
        }

        private int UserSize
        {
            get { return Context.Client.Guilds.Select(x => x.Users.Count).Sum(); }
        }

        [Command("Info"), Summary("Display information regarding the bot")]
        public async Task Info()
        {
            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder {Name = "OCRBot"},
                Title = "Bot Statistics",
                Fields = new List<EmbedFieldBuilder>
                {
                    new() {Name = "Uptime", Value = Uptime, IsInline = true},
                    new() {Name = "Users", Value = UserSize, IsInline = true},
                    new() {Name = "Servers", Value = Context.Client.Guilds.Count, IsInline = true},
                },
                Color = Color.Gold
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Command("Invite"), Summary("Show the invite link for the bot")]
        public async Task Invite()
        {
            var embed = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder
                    {IconUrl = Context.Client.CurrentUser.GetAvatarUrl(), Name = Context.Client.CurrentUser.Username},
                Title = "Invite Me!!!!",
                Url = $"https://discord.com/oauth2/authorize?client_id={Context.Client.CurrentUser.Id}&scope=bot"
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Command("translate"), Summary("Translate using googles API"), Alias("gt")]
        [Remarks("All of the translation codes can be found here (https://cloud.google.com/translate/docs/languages)")]
        public async Task GoogleTranslate(string from, string to, [Remainder] string text)
        {
            EmbedBuilder embed = await Translate.GetTranslatedEmbed(from, to, text);
            await ReplyAsync("", false, embed.Build());
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("prefix"), Summary("Change the prefix of the bot")]
        public async Task Prefix(string prefix)
        {
            var guild = await Database.LoadRecordsByGuildId(Context.Guild.Id);
            guild.Prefix = prefix;
            await Database.UpdateGuild(guild);
            await ReplyAsync("Prefix updated");
        }

        [Command("support"), Summary("Displays a link where issues and support can be received.")]
        public async Task Support()
        {
            await ReplyAsync("https://github.com/Jirubizu/OCRBot/issues");
        }
    }
}