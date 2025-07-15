


using Microsoft.EntityFrameworkCore;

namespace TiktokGame2Server.Entities
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
    }
}
