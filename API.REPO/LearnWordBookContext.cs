using API.DATA;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.REPO
{
    public class LearnWordBookContext : DbContext
    {
        public LearnWordBookContext(DbContextOptions<LearnWordBookContext> options) : base(options) { }
        #region register DbSet
        public DbSet<Account> Accounts { get; set; }
        public DbSet<LearnedWord> LearnedWords { get; set; }
        public DbSet<LoginSession> LoginSessions { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetDefaulColumn(modelBuilder);
            SetMultiForeignKey(modelBuilder);
            InitData(modelBuilder);
        }
        private void SetDefaulColumn(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>()
                        .Property(x => x.Note)
                        .HasDefaultValue("");
            modelBuilder.Entity<User>()
                        .Property(x => x.Avatar)
                        .HasDefaultValue("");
        }
        private void InitData(ModelBuilder modelBuilder)
        {
            //user
            var users = new List<User>() {
               new User
               {
                    Id=1,
                    FullName = "Learn wordbook",
                    CreateAt = DateTime.Now,
                    Avatar = "",
                    CreateBy = "",
                    Email = "admin.learn-wordbook@gmail.com",
                    IsTrash = false
               }
            };
            modelBuilder.Entity<User>().HasData(users);
            //account
            var accounts = new List<Account>
            {
                new Account
                {
                    Id=1,
                    UserId=1,
                    Username="admin",
                    Password="21232f297a57a5a743894a0e4a801fc3",
                    CreateAt=DateTime.Now,
                    CreateBy="",
                    IsTrash=false
                }
            };
            modelBuilder.Entity<Account>().HasData(accounts);

        }
        private void SetMultiForeignKey(ModelBuilder modelBuilder)
        {

        }
    }
}
