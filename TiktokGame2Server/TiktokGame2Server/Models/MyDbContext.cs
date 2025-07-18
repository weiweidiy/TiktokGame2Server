


using JFramework;
using Microsoft.EntityFrameworkCore;

namespace TiktokGame2Server.Entities
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)/*, IGameDataStore*/
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<ChapterNode> ChaptersNodes { get; set; }
        public DbSet<ChapterNodeStar> ChapterNodeStars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>().HasIndex(c => c.ChapterId).IsUnique();

            // 确保DeviceId唯一
            //modelBuilder.Entity<Account>()
            //    .HasIndex(u => u.PlayerId)
            //    .IsUnique();
        }

        //public Task ClearAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> ExistsAsync(string key)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<string>> GetAllKeysAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> GetAsync<T>(string key)
        //{
        //    //switch(nameof(T))
        //    //{
        //    //    case nameof(Player):
        //    //        return Players.FindAsync(key).Result;
        //    //}
        //    throw new NotImplementedException();
        //}

        //public Task<bool> RemoveAsync(string key)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SaveAsync<T>(string key, T value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
