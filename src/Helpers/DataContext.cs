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

            builder.Entity<Course>()//Tabela Couse para se caso deletar o curso, não deletar o professor
                .HasOne(e => e.Teacher)//Tem um professor
                .WithMany(c => c.CoursesTeaching)// Dá aula em vários cursos
                .OnDelete(DeleteBehavior.Restrict);// comportamento restrito para deletar

            //Builder para configurar relacionamento N pra N 
            builder.Entity<Course>()//tabela course: 
                .HasMany(p => p.Students) //tem vários estudantes
                .WithMany(p => p.CoursesStudind) // tem vários cursos
                .UsingEntity<StudentCourse>( //tabela intermediária
                j => j //Primeiro relacionamento da tabela intermediária
                .HasOne(pt => pt.Student)//Tem um estudante
                .WithMany(t => t.StudentCourses)//tem vários studentsCourses
                .HasForeignKey(pt => pt.StudentId),
                j => j // Segundo relacionamento da tabela intermediária
                .HasOne(pt => pt.Course)// tem um curso
                .WithMany(p => p.StudentCourses)// tem vários studentCourse
                .HasForeignKey(pt => pt.CourseId), // chave estrangeira é CourseId
                j =>
                {
                    j.HasKey(t => new { t.CourseId, t.StudentId }); //Chave primária composta da tabela intermediária
                });
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
