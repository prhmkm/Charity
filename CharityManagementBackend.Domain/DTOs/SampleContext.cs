using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.DTOs
{
    public partial class SampleContext : DbContext
    {
        public virtual DbSet<TranReportDTO> sp_TranReport { get; set; }

        public SampleContext()
        {
        }

        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TranReportDTO>(entity =>
            {
                entity.HasNoKey();
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
