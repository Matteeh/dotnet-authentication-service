using identity.Data;
using identity.Models;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}