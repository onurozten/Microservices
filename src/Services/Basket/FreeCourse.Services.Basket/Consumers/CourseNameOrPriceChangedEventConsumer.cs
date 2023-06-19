using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services;
using MassTransit;
using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Consumers
{
    public class CourseNameOrPriceChangedEventConsumer : IConsumer<CourseNameOrPriceChangedEvent>
    {
        private readonly IBasketService _basketService;
        private readonly RedisService _redisService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CourseNameOrPriceChangedEventConsumer(IBasketService basketService, RedisService redisService,
            ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _redisService = redisService;
            _sharedIdentityService = sharedIdentityService;
        }
        public async Task Consume(ConsumeContext<CourseNameOrPriceChangedEvent> context)
        {
            var server = _redisService.GetServer();
            var keys = server.Keys(1).ToList();

            foreach (var item in keys)
            {
                var basket = await _basketService.GetBasket(item);

                var basketItemm = basket.Data.BasketItems.Where(x => x.CourseId == context.Message.CourseId).FirstOrDefault();

                if(basketItemm!=null)
                {
                    basketItemm.CourseName = context.Message.UpdatedName;
                    basketItemm.Price = context.Message.UpdatedPrice;
                    await _basketService.SaveOrUpdate(basket.Data);
                }

            }

            // redis implamantasyonu uygulanacak
            //var a = await _redisService.GetDb().StringGetAsync(_sharedIdentityService.GetUserId);
         }
    }
}
