using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisHWQueueTriger.Services
{
    public class RedisService : IRedisService
    {
        public async Task AddImgInRedis(string url)
        {

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);

            string jsonData = await response.Content.ReadAsStringAsync();

            using JsonDocument document = JsonDocument.Parse(jsonData);
            JsonElement root = document.RootElement;

            string posterUrl = root.GetProperty("Poster").GetString();

            var muxer = ConnectionMultiplexer.Connect(
                   new ConfigurationOptions
                   {
                       EndPoints = { { "redis-16071.c114.us-east-1-4.ec2.redns.redis-cloud.com", 16071 } },
                       User = "default",
                       Password = "YJQ91T7S80iPSsnU5w6vD1Q7OBwrdK1x"
                   }
               );


            var db = muxer.GetDatabase();
            var subscriber = muxer.GetSubscriber();

            

            db.ListLeftPush("moveImages", posterUrl);

            subscriber.Publish("moveImages", posterUrl);

            
        }

       
    }
}
