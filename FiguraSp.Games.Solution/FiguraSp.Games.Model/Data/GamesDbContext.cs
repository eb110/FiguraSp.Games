using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Views;
using Microsoft.EntityFrameworkCore;

namespace FiguraSp.Games.Model.Data
{
    public partial class GamesDbContext(DbContextOptions<GamesDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<PicklistGameLevel> PicklistGameLevel { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<VRiderEvent> V_Rider_Events { get; set; }

        public virtual async Task<T> GetFirstOrDefaultAsync<T>(IQueryable<T> query)
        {
            var entity = await query.FirstOrDefaultAsync();

            return entity!;
        }

        public virtual async Task<List<T>> GetEntitiesToListAsync<T>(IQueryable<T> query)
        {
            var entities = await query.ToListAsync();

            return entities;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Polish_100_CS_AS_KS_WS_SC_UTF8");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasIndex(e => e.GameId, "IX_Events_GameId");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.HomeAway).HasDefaultValue("");

                entity.HasOne(d => d.Game).WithMany(p => p.Events).HasForeignKey(d => d.GameId);
            });

            modelBuilder.Entity<PicklistGameLevel>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Picklist__3214EC0708777185");

                entity.ToTable("PicklistGameLevel");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.GameLevel).HasMaxLength(50);
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<VRiderEvent>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("V_Rider_Events");

                entity.Property(e => e.Heats).HasMaxLength(4000);
                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .UseCollation("Persian_100_CS_AS_KS_WS_SC_UTF8");
                entity.Property(e => e.Rows).HasMaxLength(4000);
                entity.Property(e => e.Surname)
                    .HasMaxLength(30)
                    .UseCollation("Persian_100_CS_AS_KS_WS_SC_UTF8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
