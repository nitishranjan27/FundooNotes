using Common_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface IUserRL
    {
        public UserEntity Registration(UserReg userRegModel);
        public string Login(UserLoginModel userLogin);
    }
}