using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Membership;

namespace Stackoverflow.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>, IApplicationDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = new Guid("CAD5F124-3A9F-45FC-8553-669146DD08D3"),
                    UserId = new Guid("FE6D58B6-C61E-45D1-A797-FC97B00EF640"), // Replace with actual user ID
                    Title = "How to improve programming skills?",
                    Body = "I'm looking for tips and advice on how to become a better programmer. Any recommendations on books, online courses, or coding challenges would be greatly appreciated!",
                    PostType = PostType.Question,
                    Upvotes = 10,
                    Downvotes = 2,
                    CreationDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    IsDeleted = false
                },
                new Post
                {
                    Id = new Guid("613641BF-EA84-491A-902F-982DB227CB1A"),
                    UserId = new Guid("940A3086-054E-4DBB-8BA3-1B83DF4EE62E"), // Replace with actual user ID
                    Title = "Re: How to improve programming skills?",
                    Body = "One way to improve your programming skills is by practicing regularly. Try to solve coding problems daily and participate in coding competitions. Also, consider contributing to open-source projects on platforms like GitHub.",
                    PostType = PostType.Reply,
                    Upvotes = 15,
                    Downvotes = 1,
                    CreationDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    IsDeleted = false
                },
                new Post
                {
                    Id = new Guid("C40C756C-F117-4FA4-B7C6-A28F01EB6995"),
                    UserId = new Guid("AC7BF694-8D79-4B67-BC6C-117E661D75C9"), // Replace with actual user ID
                    Title = "Best programming language for beginners?",
                    Body = "I'm new to programming and wondering which language I should start learning first. Any recommendations based on ease of learning and job opportunities?",
                    PostType = PostType.Question,
                    Upvotes = 8,
                    Downvotes = 3,
                    CreationDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    IsDeleted = false
                },
                new Post
                {
                    Id = new Guid("4D64C300-950A-4F2B-867F-D3ED6F4F0E2C"),
                    UserId = new Guid("E7A46768-075E-41FF-8732-79BC75BF0711"), // Replace with actual user ID
                    Title = "Re: Best programming language for beginners?",
                    Body = "Python is often recommended for beginners due to its simple syntax and versatility. It's used in various domains such as web development, data science, and automation. Additionally, there are plenty of resources available online for learning Python.",
                    PostType = PostType.Reply,
                    Upvotes = 12,
                    Downvotes = 0,
                    CreationDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    IsDeleted = false
                },
                new Post
                {
                    Id = new Guid("A924E31A-0D24-46E5-A3CA-80D31A1A445B"),
                    UserId = new Guid("B8C38056-0974-4493-920F-87FF018274CD"), // Replace with actual user ID
                    Title = "How to prepare for technical interviews?",
                    Body = "I'm preparing for technical interviews and would like some advice on the best way to study and practice. What topics should I focus on, and are there any resources you recommend?",
                    PostType = PostType.Question,
                    Upvotes = 6,
                    Downvotes = 1,
                    CreationDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    IsDeleted = false
                }

            );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Post> posts { get; set; }

    }
}
