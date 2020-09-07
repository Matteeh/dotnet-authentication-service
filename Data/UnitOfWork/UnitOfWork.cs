
namespace identity.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public IUserRepository Users { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}