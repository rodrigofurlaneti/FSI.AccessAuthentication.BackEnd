using FSI.AccessAuthentication.Infrastructure.Interfaces;
using System.Data;

namespace FSI.AccessAuthentication.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IDbContext _context;

        protected BaseRepository(IDbContext context)
        {
            _context = context;
        }

        protected IDbConnection CreateConnection()
        {
            return _context.CreateConnection();
        }
    }
}
