using Microsoft.EntityFrameworkCore;
using STUDENTMANAGEMENT.DAL.SQLServerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTMANAGEMENT.DAL.DbContexts
{
    public class StudentManagementDbContext : DbContext
    {
        public StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TApplicationLog> TApplicationLog { get; set; } = null!;
        public virtual DbSet<TCity> TCity { get; set; } = null!;
        public virtual DbSet<TState> TState { get; set; } = null!;
        public virtual DbSet<TDocument> TDocument { get; set; } = null!;
        public virtual DbSet<TMarksList> TMarksList { get; set; } = null!;
        public virtual DbSet<TSubject> TSubject { get; set; } = null!;
        public virtual DbSet<TStudent> TStudent { get; set; } = null!;
        public virtual DbSet<TStudentStage> TStudentStage { get; set; } = null!;
    }
}
