﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using OCRBot.OCRBotCustoms;
using OCRBot.Services;

namespace OCRBot.Modules
{
    public class OcrModule : OCRBotCustomModule
    {
        public PaginationService Paging { get; set; }
        public OCRService OcrService { get; set; }
        
        [Command("ocr"), Summary("Scan the image provided and output the text found")]
        [Remarks("All of the translation codes can be found here (Arabic=ara, Bulgarian=bul, Chinese(Simplified)=chs, Chinese(Traditional)=cht, Croatian = hrv, Czech = cze" +
                 "Danish = dan, Dutch = dut, English = eng, Finnish = fin, French = fre, German = ger, Greek = gre, Hungarian = hun, Korean = kor, Italian = ita, Japanese = jpn, " +
                 "Polish = pol, Portuguese = por, Russian = rus, Slovenian = slv, Spanish = spa, Swedish = swe, Turkish = tur\n\n" +
                 "first arg is either a image url or a language code to translate from if an attachment is present, arg2 is the language code to translate from and should only be provided " +
                 "if a url is passed as the first argument")]
        public async Task Ocr(string arg = null, string arg2 = null)
        {
            List<EmbedBuilder> pages;
            const string regex = "(http(s?):)|([/|.|\\w|\\s])*\\.(?:jpe?g|gif|png)";
            
            if (TryGetAttachment(Context, out string attachmentUrl))
            {
                if (!Regex.IsMatch(attachmentUrl, regex)) return;
                if (arg != null)
                {
                    pages = await OcrService.OcrTranslate(attachmentUrl, arg);
                }
                else
                {
                    await (ReplyAsync(await OcrService.Ocr(attachmentUrl)));
                    return;
                }
            }
            else if (arg != null &&  Regex.IsMatch(arg, regex))
            {
                if (arg2 != null)
                {
                    pages = await OcrService.OcrTranslate(arg, arg2);
                }
                else
                {
                    string result = await OcrService.Ocr(arg);
                    await ReplyAsync(result);
                    return;
                }
            }
            else
            {
                await ReplyAsync("```Incorrect use of the command.```");
                return;
            }

            PaginatedMessage paginator = new PaginatedMessage(pages, "OCR Translated", Color.Teal, Context.User, new AppearanceOptions{Timeout = TimeSpan.FromMinutes(10), Style = DisplayStyle.Minimal});
            await Paging.SendMessageAsync(Context.Channel, paginator);
        }
    }
}