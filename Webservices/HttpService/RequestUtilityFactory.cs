using System;
using CasCap.Apis.TokenBucket;

namespace Webservices.HttpService
{
    public static class RequestUtilityFactory
    {


        public static ITokenBucket GetTokenBucket(int refilIntervalSeconds = 5)
        {
            return TokenBuckets.Construct()
              .WithCapacity(1)
              .WithFixedIntervalRefillStrategy(1, TimeSpan.FromSeconds(refilIntervalSeconds))
              .Build();
        }
    }
}