using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OCRBot.Structs
{
    public class GuildBson
    {
        [BsonId]
        public ObjectId CollectionId { get; set; }

        [BsonElement("guild_id")]
        public ulong GuildId { get; set; }
        
        [BsonElement("prefix")]
        public string Prefix { get; set; }
    }
}