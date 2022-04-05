using Common_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buisness_Layer.Interface
{
    public interface IUserBL
    {
        public UserEntity Registration(UserReg userRegModel);
       //public string Login(UserLoginModel userLogin);
        public LoginResponse UserLogin(UserLoginModel userLog);
    }
}
