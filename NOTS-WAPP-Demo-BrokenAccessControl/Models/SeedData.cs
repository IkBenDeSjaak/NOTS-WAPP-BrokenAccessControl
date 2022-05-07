using Microsoft.EntityFrameworkCore;
using NOTS_WAPP_Demo_BrokenAccessControl.Data;

namespace NOTS_WAPP_Demo_BrokenAccessControl.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new NOTS_WAPP_Demo_BrokenAccessControlContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<NOTS_WAPP_Demo_BrokenAccessControlContext>>()))
            {
                // Look for any movies.
                if (context.Blog.Any())
                {
                    return;   // DB has been seeded
                }

                context.Blog.AddRange(
                    new Blog
                    {
                        Title = "First Blog",
                        Date = DateTime.Parse("1989-2-12"),
                        Body = "Very nice blog"
                    },

                    new Blog
                    {
                        Title = "Second Blog",
                        Date = DateTime.Parse("1991-2-13"),
                        Body = "Very very nice blog"
                    },

                    new Blog
                    {
                        Title = "Another Blog",
                        Date = DateTime.Parse("1995-4-12"),
                        Body = "Very very very nice blog"
                    },

                    new Blog
                    {
                        Title = "Blogger",
                        Date = DateTime.Parse("1999-11-19"),
                        Body = "Blogdog"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
