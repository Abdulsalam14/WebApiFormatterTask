using Microsoft.EntityFrameworkCore;
using WebApiDemo1225.Entities;

namespace WebApiDemo1225.Data
{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            :base(options)
        {
        }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

    }
}
