    using Microsoft.EntityFrameworkCore;
    using WebApplication1.Models;

    namespace WebApplication1.Data;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<GameLevel> GameLevels { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<LevelResult> LevelResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Region
            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.regionId);
                entity.Property(e => e.Name).IsRequired();
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.roleId);
                entity.Property(e => e.Name).IsRequired();
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasQueryFilter(u => !u.IsDeleted); // Soft delete filter
                entity.HasKey(e => e.userId);
                entity.Property(e => e.username).IsRequired();
                entity.Property(e => e.linkAvatar);
                entity.Property(e => e.otp);

                entity.HasOne(e => e.region)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.regionId)
                    .IsRequired();

                entity.HasOne(e => e.role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.roleId)
                    .IsRequired();
            });

            // GameLevel
            modelBuilder.Entity<GameLevel>(entity =>
            {
                entity.HasKey(e => e.GameLevelId);
                entity.Property(e => e.Title).IsRequired();

                entity.HasMany(e => e.Questions)
                    .WithOne(q => q.GameLevel)
                    .HasForeignKey(q => q.GameLevelId);

                entity.HasMany(e => e.LevelResults)
                    .WithOne(lr => lr.GameLevel)
                    .HasForeignKey(lr => lr.GameLevelId);
            });

            // Question
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.QuestionId);
                entity.Property(q => q.Content).IsRequired();
                entity.Property(q => q.Answer).IsRequired();
            });

            // LevelResult
            modelBuilder.Entity<LevelResult>(entity =>
            {
                entity.HasKey(lr => lr.LevelResultId);

                entity.HasOne(lr => lr.User)
                    .WithMany(u => u.LevelResults)
                    .HasForeignKey(lr => lr.UserId);

                entity.HasOne(lr => lr.GameLevel)
                    .WithMany(gl => gl.LevelResults)
                    .HasForeignKey(lr => lr.GameLevelId);
            });

            // --- Seed Data ---

            // Regions
            modelBuilder.Entity<Region>().HasData(
                new Region(1, "Region1"),
                new Region(2, "Region2")
            );

            // Roles
            modelBuilder.Entity<Role>().HasData(
                new Role(1, "Admin"),
                new Role(2, "User")
            );

            // Users
            modelBuilder.Entity<User>().HasData(
        new User {
            userId = 1,
            username = "user1",
            linkAvatar = "avatar1.png",
            otp = "123456",
            regionId = 1,
            roleId = 1,
            Email = "admin@example.com",
            IsDeleted = false,
            Password = ""
        },
        new User {
            userId = 2,
            username = "user2",
            linkAvatar = "avatar2.png",
            otp = "654321",
            regionId = 2,
            roleId = 2,
            Email = "user@example.com",
            IsDeleted = false,
            Password = ""
        }
    );


            // GameLevels
            modelBuilder.Entity<GameLevel>().HasData(
                new GameLevel(1, "Level 1", "Mức độ dễ"),
                new GameLevel(2, "Level 2", "Mức độ trung bình"),
                new GameLevel(3, "Level 3", "Mức độ khó")
            );

            // Questions
            modelBuilder.Entity<Question>().HasData(
                new { QuestionId = 1, Content = "Question 1 of Level 1?", Answer = "A", OptionA = "A", OptionB = "B", OptionC = "C", OptionD = "D", GameLevelId = 1 },
                new { QuestionId = 2, Content = "Question 2 of Level 1?", Answer = "B", OptionA = "A", OptionB = "B", OptionC = "C", OptionD = "D", GameLevelId = 1 },
                new { QuestionId = 3, Content = "Question 1 of Level 2?", Answer = "C", OptionA = "A", OptionB = "B", OptionC = "C", OptionD = "D", GameLevelId = 2 },
                new { QuestionId = 4, Content = "Question 2 of Level 2?", Answer = "D", OptionA = "A", OptionB = "B", OptionC = "C", OptionD = "D", GameLevelId = 2 },
                new { QuestionId = 5, Content = "Question 1 of Level 3?", Answer = "A", OptionA = "A", OptionB = "B", OptionC = "C", OptionD = "D", GameLevelId = 3 }
            );

            // LevelResults
        modelBuilder.Entity<LevelResult>().HasData(
        new LevelResult { LevelResultId = 1, UserId = 1, GameLevelId = 1, Score = 80, CompletionDate = new DateTime(2025,11,19) },
        new LevelResult { LevelResultId = 2, UserId = 1, GameLevelId = 2, Score = 60, CompletionDate = new DateTime(2025,11,20) },
        new LevelResult { LevelResultId = 3, UserId = 2, GameLevelId = 1, Score = 90, CompletionDate = new DateTime(2025,11,21) }
    );

        }
    }
