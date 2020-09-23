using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Discord.Commands;

namespace OCRBot.Utilities.Comparers
{
    public class CommandInfoComparer : IEqualityComparer<CommandInfo>
    {
        public bool Equals(CommandInfo x, CommandInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Name == y.Name;
        }

        public int GetHashCode(CommandInfo obj)
        {
            return obj.Name == null ? 0 : obj.Name.GetHashCode();
        }
        
        // public int GetHashCode([DisallowNull] CommandInfo obj)
        // {
        //     return (obj.Name.Length * (obj.Summary?.Length ?? 5)) ^ obj.Parameters.Count;
        // }
    }
}