using Microsoft.EntityFrameworkCore;
using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy
{
    public interface IPermissionRepository<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        Task<List<Permission<TPerm, TPermCat>>> GetAllPermissionsAsync();
        Task<List<PermissionCategory<TPerm, TPermCat>>> GetAllPermissionCategoriesAsync();
    }

    public class PermissionRepository<TPerm, TPermCat> : IPermissionRepository<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly DbContext _context;

        public PermissionRepository(DbContext context)
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