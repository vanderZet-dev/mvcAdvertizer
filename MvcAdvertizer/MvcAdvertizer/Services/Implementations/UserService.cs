using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUsers userRepository;

        public UserService(IUsers userRepository) {
            this.userRepository = userRepository;
        }

        public async Task<User> Create(User obj) {
            return await userRepository.Add(obj);
        }

        public async Task Delete(User obj) {

            await userRepository.Delete(obj);
        }

        public IQueryable<User> FindAll() {

            return userRepository.FindAll();
        }

        public async Task<User> FindById(Guid guid) {

            return await userRepository.FindById(guid);
        }

        public async Task<User> Update(User obj) {

            return await userRepository.Update(obj);
        }
    }
}
