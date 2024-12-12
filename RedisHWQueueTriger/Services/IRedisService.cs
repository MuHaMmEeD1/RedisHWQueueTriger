using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHWQueueTriger.Services
{
    public interface IRedisService
    {
        Task AddImgInRedis(string url);
    }
}
