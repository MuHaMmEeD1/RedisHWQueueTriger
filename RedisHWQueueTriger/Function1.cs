using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RedisHWQueueTriger.Services;
using System;

namespace RedisHWQueueTriger
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRedisService _redisService;

        public Function1(ILogger<Function1> logger, IConfiguration configuration, IRedisService redisService)
        {
            _logger = logger;
            _configuration = configuration;
            _redisService = redisService;
        }

        [Function(nameof(Function1))]
        public async Task Run([QueueTrigger("redis",Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processing: {message.MessageText}");

            try
            {
                string key = _configuration["Values:OMDBApiKey"];
                string url = $"http://www.omdbapi.com/?apikey={key}&t={message.MessageText}";

                await _redisService.AddImgInRedis(url);

                _logger.LogInformation("Image URL successfully added to Redis.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {message.MessageText}. Error: {ex.Message}");
            }
        }

    }
}
