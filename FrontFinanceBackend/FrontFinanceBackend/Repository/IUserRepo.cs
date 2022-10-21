using FrontFinanceBackend.Models;

namespace FrontFinanceBackend.Repository
{
    public interface IUserRepo
    {
        Task<FrontUser> FindById(Guid id);

        Task<List<FrontUser>> GetAll();
    }
}