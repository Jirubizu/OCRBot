using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using OCRBot.Structs;

namespace OCRBot.Services
{
    public class DatabaseService
    {
        private readonly IMongoDatabase _mongoDatabase;

        private readonly Dictionary<ulong, GuildBson>
            _matsueGuildCache = new Dictionary<ulong, GuildBson>();

        public DatabaseService(DiscordShardedClient shardedClient, ConfigService configService)
        {
            var client = new MongoClient(configService.Config.MongoDBUri);
            _mongoDatabase = client.GetDatabase(configService.Config.MongoDBName);

            shardedClient.JoinedGuild += OnJoinedGuild;
            shardedClient.LeftGuild += OnLeftGuild;
        }

        // Loading Records
        public async Task<GuildBson> LoadRecordsByGuildId(ulong guildId)
        {
            if (_matsueGuildCache.TryGetValue(guildId, out var cacheGuild))
            {
                return cacheGuild;
            }

            var collection = _mongoDatabase.GetCollection<GuildBson>("guilds");
            var filter = Builders<GuildBson>.Filter.Eq("guild_id", guildId);

            GuildBson guild;
            
            try
            {
                guild = await collection.Find(filter).FirstAsync();
            }
            catch
            {
                guild = new GuildBson {Prefix = ";", GuildId = guildId};
                await InsertRecord("guilds", guild);
            }
            
            _matsueGuildCache.Add(guild.GuildId, guild);
            return guild;
        }

        // Inserting Records
        public async Task InsertRecord<T>(string table, T record)
        {
            switch (record)
            {
                case GuildBson guild:
                    _matsueGuildCache.Add(guild.GuildId, guild);
                    break;
            }

            var collection = _mongoDatabase.GetCollection<T>(table);
            await collection.InsertOneAsync(record);
        }

        // Updating Records

        public async Task UpdateGuild(GuildBson record)
        {
            _matsueGuildCache[record.GuildId] = record;
            var collection = _mongoDatabase.GetCollection<GuildBson>("guilds");
            var filter = Builders<GuildBson>.Filter.Eq("guild_id", record.GuildId);
            await collection.ReplaceOneAsync(filter, record);
        }

        // Delete Records

        private async Task DeleteGuildRecord(ulong guildId)
        {
            _matsueGuildCache.Remove(guildId);
            var collection = _mongoDatabase.GetCollection<GuildBson>("guilds");
            var filter = Builders<GuildBson>.Filter.Eq("guild_id", guildId);
            await collection.DeleteOneAsync(filter);
        }

        // Events

        private async Task OnJoinedGuild(SocketGuild arg)
        {
            await InsertRecord("guilds",
                new GuildBson
                {
                    GuildId = arg.Id, Prefix = ";"
                });
        }

        private async Task OnLeftGuild(SocketGuild arg)
        {
            await DeleteGuildRecord(arg.Id);
        }
    }
}