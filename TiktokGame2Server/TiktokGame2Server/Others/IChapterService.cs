using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IChapterService
    {
        Task<Chapter?> GetChapterAsync(int id);
        Task<List<Chapter>> CreateChaptersAsync(int playerId);

    }
}
