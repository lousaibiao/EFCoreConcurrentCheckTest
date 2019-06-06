using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreDemo.Data.Model
{
    public partial class JhwContext : DbContext
    {
        public JhwContext()
        {
        }

        public JhwContext(DbContextOptions<JhwContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Test1> Test1 { get; set; }
        public virtual DbSet<Test2> Test2 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test1>(entity =>
            {
                entity.ToTable("test1");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name3).HasColumnType("varchar(50)");

                entity.Property(e => e.Name4).HasColumnType("datetime");

                entity.Property(e => e.Name5).HasColumnType("decimal(18,2)");

                entity.Property(e => e.RowVersion)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate()
                    .IsConcurrencyToken()
                    ;

                entity.Property(e => e.Val1).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Test2>(entity =>
            {
                entity.ToTable("test2");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name2).HasColumnType("varchar(50)");

                entity.Property(e => e.Name3).HasColumnType("varchar(50)");

                entity.Property(e => e.Name4).HasColumnType("datetime");

                entity.Property(e => e.Name5).HasColumnType("decimal(10,0)");

                entity.Property(e => e.Val1).HasColumnType("int(11)");
            });
        }
    }
}
