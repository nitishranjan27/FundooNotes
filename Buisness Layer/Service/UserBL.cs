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

        public string ForgetPassword(string email)
        {
            try
            {
                return userRL.ForgetPassword(email);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool ResetPassword(string email, string password, string confirmPassword)
        {
            try
            {
                return this.userRL.ResetPassword(email, password, confirmPassword);
            }
            catch (Exception)
            {

                throw;
            }
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

        public LoginResponse UserLogin(UserLoginModel userLog)
        {
            try
            {
                return this.userRL.UserLogin(userLog);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //string IUserBL.Login(UserLoginModel userLogin)
        //{
        //    try
        //    {
        //        return userRL.Login(userLogin);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
