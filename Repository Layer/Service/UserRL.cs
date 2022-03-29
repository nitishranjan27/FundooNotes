using Common_Layer.Models;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository_Layer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public UserEntity Registration(UserReg userRegModel)
        {
            try
            {
                UserEntity newUser = new UserEntity();
                newUser.FirstName = userRegModel.FirstName;
                newUser.LastName = userRegModel.LastName;
                newUser.Email = userRegModel.Email;
                newUser.Password = userRegModel.Password;
                fundooContext.Add(newUser);
                int result = fundooContext.SaveChanges();
                if (result > 0)
                {
                    return newUser;
                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string Login(UserLoginModel userLogin)
        {
            try
            {
                var LoginResult = this.fundooContext.UserTable.Where(X => X.Email == userLogin.Email && X.Password == userLogin.Password).FirstOrDefault();
                if (LoginResult != null)
                {
                    return LoginResult.Email;
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }
    }
}
