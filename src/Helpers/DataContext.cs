using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Enums;

namespace SchoolOfDevs.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(e => e.TypeUser)
                .HasConversion(
                   v => v.ToString(),
                   v => (TypeUser)Enum.Parse(typeof(TypeUser),v)); // Converte o Enum para string para o banco receber o nome e não o número
                   
        }

        //Sobescrevendo o método SaveChangesAsync para atualizar a data de criação e de atualização
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity &&(
                 e.State == EntityState.Added
                 || e.State == EntityState.Modified
                 ));
            foreach (var entry in entries)
            {
                DateTime dateTime = DateTime.Now;
                ((BaseEntity)entry.Entity).UpdateAt = dateTime;

                if(entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedAt = dateTime;
                }
            } 

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
