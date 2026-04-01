using FiguraSp.Games.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace FiguraSp.Games.Model.Data
{
    public partial class GamesDbContext(DbContextOptions<GamesDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Season> Seasons { get; set; }
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
    }
}
