using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class ChapterService : IChapterService
    {
        private readonly MyDbContext _dbContext;
        public ChapterService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Chapter?> GetChapterAsync(int id)
        {
            return await _dbContext.Chapters.FindAsync(id);
        }

        

        public async Task<List<Chapter>> CreateChaptersAsync(int playerId)
        {
            var chapters = GetAllChapters();
            var result = new List<Chapter>();
            foreach (var id in chapters)
            {
                var chapter = new Chapter
                {
                    ChapterId = id,
                    PlayerId = playerId,
                    // 其他属性初始化
                };

                _dbContext.Chapters.Add(chapter);
                result.Add(chapter);
            }

           
            
            await _dbContext.SaveChangesAsync();
            return result;
        }


        List<int> GetAllChapters()
        {
            return new List<int>() { 1,2};
        }
    }
}
