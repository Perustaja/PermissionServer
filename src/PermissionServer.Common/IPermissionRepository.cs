using Microsoft.EntityFrameworkCore;

namespace PermissionServer.Common
{
    public interface IPermissionRepository<TContext, TPerm, TPermCat>
        where TContext : DbContext
        where TPerm : Enum
        where TPermCat : Enum
    {
        Task<List<Permission<TPerm, TPermCat>>> GetAllPermissionsAsync();
        Task<List<PermissionCategory<TPerm, TPermCat>>> GetAllPermissionCategoriesAsync();
    }

    public class PermissionRepository<TContext, TPerm, TPermCat> : IPermissionRepository<TContext, TPerm, TPermCat>
        where TContext : DbContext
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly TContext _context;

        public PermissionRepository(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Permission<TPerm, TPermCat>>> GetAllPermissionsAsync()
            => await _context.Set<Permission<TPerm, TPermCat>>()
                .Include(p => p.PermCategory)
                .ToListAsync();

        public async Task<List<PermissionCategory<TPerm, TPermCat>>> GetAllPermissionCategoriesAsync()
            => await _context.Set<PermissionCategory<TPerm, TPermCat>>()
                .Include(pc => pc.Permissions)
                .ToListAsync();
    }
}