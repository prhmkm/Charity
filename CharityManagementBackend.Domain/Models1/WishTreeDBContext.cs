using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CharityManagementBackend.Domain.Models
{
    public partial class WishTreeDBContext : DbContext
    {
        public WishTreeDBContext()
        {
        }

        public WishTreeDBContext(DbContextOptions<WishTreeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CommentPicture> CommentPictures { get; set; } = null!;
        public virtual DbSet<CommentState> CommentStates { get; set; } = null!;
        public virtual DbSet<DWH_Transaction> DWH_Transactions { get; set; } = null!;
        public virtual DbSet<PersonalityType> PersonalityTypes { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionState> TransactionStates { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<View> Views { get; set; } = null!;
        public virtual DbSet<Wish> Wishes { get; set; } = null!;
        public virtual DbSet<WishPriority> WishPriorities { get; set; } = null!;
        public virtual DbSet<WishState> WishStates { get; set; } = null!;
        public virtual DbSet<Wisher> Wishers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=185.255.88.106,2019; initial catalog=WishTreeDB;User Id=Vira;Password=P@3w0rd!;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Persian_100_CI_AI");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<CommentPicture>(entity =>
            {
                entity.Property(e => e.Picture).IsUnicode(false);

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentPictures)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentPictures_Comments");
            });

            modelBuilder.Entity<CommentState>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<DWH_Transaction>(entity =>
            {
                entity.ToTable("DWH_Transaction");

                entity.HasIndex(e => e.TransDate, "IX_Date_1");

                entity.HasIndex(e => e.TransDate, "IX_Date_2")
                    .IsUnique();

                entity.Property(e => e.TransAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TransDate).HasColumnType("date");
            });

            modelBuilder.Entity<PersonalityType>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionState>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.PassWord).IsUnicode(false);

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<View>(entity =>
            {
                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName).HasMaxLength(150);

                entity.Property(e => e.LastName).HasMaxLength(150);

                entity.Property(e => e.UserId)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Wish>(entity =>
            {
                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrentAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.GoalAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Picture).IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StateId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Wishes)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wishes_WishPriority");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Wishes)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wishes_WishStates");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wishes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wishes_Users");

                entity.HasOne(d => d.Wisher)
                    .WithMany(p => p.Wishes)
                    .HasForeignKey(d => d.WisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wishes_Wishers");
            });

            modelBuilder.Entity<WishPriority>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<WishState>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<Wisher>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IbanNumber)
                    .HasMaxLength(26)
                    .IsUnicode(false);

                entity.Property(e => e.IbanOwner).HasMaxLength(150);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NationalId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Picture).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
