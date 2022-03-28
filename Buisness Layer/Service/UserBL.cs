using Buisness_Layer.Interface;
using Common_Layer.Models;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buisness_Layer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public UserEntity Registration(UserReg userRegModel)
        {
            try
            {
                return userRL.Registration(userRegModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
