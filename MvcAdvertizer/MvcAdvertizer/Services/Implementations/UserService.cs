using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Services.Interfaces;
using System;
using System.Linq;

namespace MvcAdvertizer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUsers userRepository;

        public UserService(IUsers userRepository) {
            this.userRepository = userRepository;
        }

        public User Create(User obj) {

            return userRepository.Add(obj);            
        }

        public void Delete(User obj) {
            
            userRepository.Delete(obj);
        }

        public IQueryable<User> FindAll() {

            return userRepository.FindAll();            
        }

        public User FindById(Guid guid) {

            return userRepository.FindById(guid);
        }

        public User Update(User obj) {

            return userRepository.Update(obj);
        }
    }
}
