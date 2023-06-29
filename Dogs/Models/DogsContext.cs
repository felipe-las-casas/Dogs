using Microsoft.EntityFrameworkCore;

namespace Dogs.Models;

public class DogsContext: DbContext
{
    public DogsContext(DbContextOptions<DogsContext> options)
        : base(options)
    { 
    }

    public DbSet<DogsItem> DogsItem { get; set; } = null!;
}
