using Microsoft.EntityFrameworkCore;
using VSAND.Arc.Data.Entities;

namespace VSAND.Arc.Data
{
    public partial class ArcContentContext : DbContext
    {
        public ArcContentContext()
        {
        }

        public ArcContentContext(DbContextOptions<ArcContentContext> options) : base(options)
        {
        }

        public virtual DbSet<ContentOperation> ContentOperation { get; set; }
        public virtual DbSet<ContentTag> ContentTag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<ContentOperation>(entity =>
            {
                entity.HasIndex(e => e.LastOperationDateUtc)
                    .HasName("idx_ContentOperation_LastOperationDate");

                entity.Property(e => e.ContentOperationId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BodyType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ByLine)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CanonicalUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ContentOperationObject)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.FeatureImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Headline)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastOperation)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastOperationDateUtc).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.Publication)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PublishDateUtc).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContentTag>(entity =>
            {
                entity.HasKey(e => new { e.ContentOperationId, e.Tag, e.Publication });

                entity.Property(e => e.ContentOperationId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tag)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Publication)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContentOperation)
                    .WithMany(p => p.ContentTags)
                    .HasForeignKey(d => d.ContentOperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContentTag_ContentOperation");
            });
        }
    }
}
