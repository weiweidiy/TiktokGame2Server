


using JFramework;
using Microsoft.EntityFrameworkCore;

namespace TiktokGame2Server.Entities
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)/*, IGameDataStore*/
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<LevelNode> LevelNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LevelNode>().HasIndex(c => new { c.NodeUid, c.PlayerId }).IsUnique();

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
