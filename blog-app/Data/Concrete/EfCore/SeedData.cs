using BlogApp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Web", Url = "web", Color = Tag.TagColors.primary },
                        new Tag { Text = "PHP", Url = "php", Color = Tag.TagColors.secondary },
                        new Tag { Text = "ASP.NET", Url = "asp-net", Color = Tag.TagColors.success },
                        new Tag { Text = "Django", Url = "django", Color = Tag.TagColors.info },
                        new Tag { Text = "JS", Url = "javascript", Color = Tag.TagColors.warning }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "theuser1", Name = "User 1", Email = "user1@gmail.com", Password = "123456", Image = "p1.jpg" },
                        new User { UserName = "theuser2", Name = "User 2", Email = "user2@gmail.com", Password = "123456", Image = "p2.jpg" }
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "ASP.NET Core",
                            Description = "ASP.NET Core Description",
                            Content = "ASP.NET Core Dersi",
                            Image = "1.jpg",
                            Url = "asp-net",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            UserId = 1,
                            Comments = new List<Comment> {
                                new Comment {
                                    Text = "Yorum 1",
                                    PublishedOn = DateTime.Now.AddDays(-14),
                                    UserId = 1
                                },
                                new Comment {
                                    Text = "Yorum 2",
                                    PublishedOn = DateTime.Now.AddDays(-24),
                                    UserId = 2
                                }
                            }
                        },
                        new Post
                        {
                            Title = "PHP",
                            Description = "PHP Description",
                            Content = "PHP Dersi",
                            Image = "2.jpg",
                            Url = "php",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-14),
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "JS",
                            Description = "JS Description",
                            Content = "JavaScript Dersi",
                            Image = "3.jpg",
                            Url = "javascript",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-7),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}