using FrontFinanceBackend.Config;
using FrontFinanceBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FrontFinanceBackend.Repository
{
    public class UserRepo : IUserRepo
    {
        public ApplicationDbContext dbContext;

        public UserRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<FrontUser> FindById(Guid id)
        {
            var dbUser = dbContext.FrontUsers.FirstOrDefaultAsync(u => u.Id == id.ToString());
            return dbUser;
        }

        public async Task<List<FrontUser>> GetAll()
        {
            return await dbContext.Set<FrontUser>().ToListAsync();
        }
    }
}