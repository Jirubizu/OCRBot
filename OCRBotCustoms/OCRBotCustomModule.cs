using System.Linq;
using Discord.Commands;

namespace OCRBot.OCRBotCustoms
{
    public class OCRBotCustomModule : ModuleBase<ShardedCommandContext>
    {
        protected bool TryGetAttachment(ShardedCommandContext context, out string result)
        {
            try
            {
                var attachment = context.Message.Attachments.FirstOrDefault();
                if (attachment != null && attachment.Width > 0 && attachment.Height > 0)
                {
                    result = attachment.Url;
                    return true;
                }

                result = null;
                return false;
            }
            catch
            {
                // ignored
            }

            result = null;
            return false;
        }
    }
}