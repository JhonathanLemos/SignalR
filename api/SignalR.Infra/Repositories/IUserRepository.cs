

using NetCoreAPI.Models;

namespace NetCoreAPI.Infra.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();
        Task<User> Add(User entity);
        Task<User> GetById(string id);
        Task<User> Update(User entity);
        Task Delete(User entity);
    }
}
