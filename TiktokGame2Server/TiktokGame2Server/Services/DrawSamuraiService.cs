using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class DrawSamuraiService : IDrawSamuraiService
    {
        private readonly ISamuraiService samuraiService;
        public DrawSamuraiService(ISamuraiService samuraiService)
        {
            this.samuraiService = samuraiService ?? throw new ArgumentNullException(nameof(samuraiService));
        }
        public async Task<Samurai> DrawSamurai(int playerId)
        {
            var businessId = "1"; // This should be replaced with the actual business ID logic
            var soldierBusinessId = "1"; // This should be replaced with the actual soldier UID logic

            var samurai = await samuraiService.AddSamuraiAsync(businessId, soldierBusinessId, playerId);

            // 广播消息到所有客户端
            //await hubContext.Clients.All.SendAsync("ReceiveDrawSamurai", new
            //{
            //    PlayerId = playerId,
            //    
            //    SamuraiBusinessId = businessId,
            //    
            //});
            return samurai;
        }

        public Task<List<Samurai>> DrawSamurais(int playerId, int count)
        {
            var businessIds = new List<string> { "1", "1", "2" }; 
            var soldierBusinessIds = new List<string> { "1", "1", "1" };
            return samuraiService.AddSamuraisAsync(businessIds, soldierBusinessIds, playerId);

        }
    }
}
