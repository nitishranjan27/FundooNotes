using Common_Layer.Models;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
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
    }
}
