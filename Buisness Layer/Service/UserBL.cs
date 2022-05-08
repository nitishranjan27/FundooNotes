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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRL">userRL Parameter</param>
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        /// <summary>
        ///  Method for Forget Password in UserBL class
        /// </summary>
        /// <param name="email">email Parameter</param>
        /// <returns></returns>
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
        /// <summary>
        /// Method for Reset Password in UserBL class
        /// </summary>
        /// <param name="email">email Parameter</param>
        /// <param name="password">password Parameter</param>
        /// <param name="confirmPassword">confirmPassword Parameter</param>
        /// <returns></returns>
        public bool ResetPassword(string email, ResetPassword resetPassword)
        {
            try
            {
                return this.userRL.ResetPassword(email, resetPassword);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method for Registration in UserBL class
        /// </summary>
        /// <param name="userRegModel">userRegModel Parameter</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for Login in UserBL class
        /// </summary>
        /// <param name="userLog">userLog Parameter</param>
        /// <returns></returns>
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
