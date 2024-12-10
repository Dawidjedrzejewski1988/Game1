using Microsoft.EntityFrameworkCore;
using Game.Models;

namespace Game.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<DailyQuest> DailyQuests { get; set; }
        public DbSet<CraftingRecipe> CraftingRecipes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamQuest> TeamQuests { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<HeroQuest> HeroQuests { get; set; } 
        public DbSet<ForumThread> ForumThreads { get; set; } 
        public DbSet<Reply> Replies { get; set; } 
    }
}

