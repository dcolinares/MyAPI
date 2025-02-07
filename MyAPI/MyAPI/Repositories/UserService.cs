using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MyAPI.Model;
using MyAPI.Application;

namespace MyAPI.Repositories
{
    public class UserService
    {
        private readonly ApplicationDbContext _appDbContext;

        // Constructor to inject AppDbContext
        public UserService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public UserService()
        {
            
        }
        // Method to fetch user details
        public UserModel? GetUser(string userName, string password)
        {
            var user = _appDbContext.Users
                .FirstOrDefault(x =>
                    x.FirstName.Equals(userName) &&
                    x.Password == password); // Adjust based on password hashing if implemented
            
            if (user != null)
            {
                UpdateUserToken(user);
            }
            return user;
        }

        //public string CheckToken(UserModel user)
        //{
        //    string errorMessage = "";
        //    if (TokenIsExpired(user.Token))
        //    {
        //        return errorMessage = "Token is expired!";
        //    } else
        //    {
        //        UpdateUserToken(user);
        //    }
        //    return errorMessage;
        //}

        public void UpdateUserToken(UserModel user)
        {
            user.Token = Guid.NewGuid().ToString();
            user.TokenExpirationDate = DateTime.Now;
            _appDbContext.SaveChanges();
        }

        //public bool TokenIsExpired(string token)
        //{
        //    var tokenIsExpired = _appDbContext.Users
        //        .Where(x => x.Token.Equals(token) && x.TokenExpirationDate >= DateTime.UtcNow.AddMinutes(-20))
        //        .Select(x => x.TokenExpirationDate)
        //        .FirstOrDefault();
        //    return tokenIsExpired != null;
        //}

        public async Task<string> SaveCreateUser(UserModel user)
        {
            string result = "";
            try 
            {
                _appDbContext.Users.Add(user);
                await _appDbContext.SaveChangesAsync();
                result = "";
            }
            catch(Exception ex)
            {
                result = ex.Message.ToString();
            }

            return result;
        }

        public string ResetPassword(int userID, string password)
        {
            string results = "";
            try
            {
                var user = _appDbContext.Users.Where(x=>x.Id.Equals(userID)).FirstOrDefault();
                if (user != null)
                {
                    user.Password = password;
                    _appDbContext.SaveChanges();

                } else 
                {
                    results = "Invalid users!";
                }
            } catch (Exception ex) 
            { 
                results += ex.Message;
            }

            return results;
        }

        public UserModel? CurrentUser { get; set; }
    }
}
