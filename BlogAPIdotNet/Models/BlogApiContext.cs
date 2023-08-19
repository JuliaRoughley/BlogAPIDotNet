using Microsoft.EntityFrameworkCore;

namespace BlogApp.Models
{
    public class BlogApiContext: DbContext
    {
        public BlogApiContext(DbContextOptions<BlogApiContext> options) : base(options)
        { }
        
        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
