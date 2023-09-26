using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApp.Models;

public class AuthServiceRepository
{
    private readonly AuthenticationDBContext _authcontext;
    private readonly List<User> _inMemoryDatabase = new List<User>();
    public AuthServiceRepository(AuthenticationDBContext context)
    {
        _authcontext = context;
    }
    public void Add(AuthenticationModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (_authcontext.authenticationModels.Any(i => i.Email == item.Email))
        {
            
            throw new InvalidOperationException("Item with the same Id already exists.");
        }
        RegisterUser(item.Email,item.Password);
        _authcontext.authenticationModels.Add(item);
        _authcontext.SaveChanges();
    }
    public bool RegisterUser(string email, string password)
    {
       
        if (_inMemoryDatabase.Any(u => u.Email == email))
        {
            return false; 
        }

        // Create a new user
        var newUser = new User
        {
            Email = email,
            Password = password 
        };

        _inMemoryDatabase.Add(newUser);

        return true; 
    }

    public bool AuthenticateUser(string email, string password)
    {
        var user = _inMemoryDatabase.FirstOrDefault(u => u.Email == email);

        return user != null && user.Password == password;
    }

    public interface IUserService
    {
        List<User> GetAllUsers();
        void AddUser(User user);
       
    }

    public class UserService : IUserService
    {
        private readonly List<User> _inMemoryDatabase = new List<User>();

        public List<User> GetAllUsers()
        {
            return _inMemoryDatabase;
        }

        public void AddUser(User user)
        {
            _inMemoryDatabase.Add(user);
        }

        
    }

}
