using ERP_System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Data.context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Studentinfo> studentdetails {  get; set; }
        public DbSet<Faculty> facultydetails { get; set; }
        public DbSet<Streams> streamdetails { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<CollegeAdmin> collegeadmins { get;set; }
        public DbSet<FacultySubject> facultysubjects { get; set; }
        public DbSet<Attendence> attences { get; set; } 
        public DbSet<Marks> marks { get; set; }
        public DbSet<InternalExam> internals { get; set; }
        public DbSet<Semester> semesters { get; set; }

    }
}
