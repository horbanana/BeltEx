using Microsoft.EntityFrameworkCore;
using Exam2.Models;

namespace Exam2.Models {
    public class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<ActivityModel> Activities {get;set;}
        public DbSet<UserActivity> UserActivities {get;set;}
    }
}