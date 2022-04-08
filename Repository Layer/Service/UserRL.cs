using Common_Layer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Repository_Layer.Service  
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration appsettings;
        public UserRL(FundooContext fundooContext, IConfiguration Appsettings)
        {
            this.fundooContext = fundooContext;
            this.appsettings = Appsettings;
        }
        public UserEntity Registration(UserReg userRegModel)
        {
            try
            {
                UserEntity newUser = new UserEntity();
                newUser.FirstName = userRegModel.FirstName;
                newUser.LastName = userRegModel.LastName;
                newUser.Email = userRegModel.Email;
                newUser.Password = EncryptPassword(userRegModel.Password);
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

        private string EncryptPassword(string password)
        {
            string enteredpassword = "";
            byte[] hide = new byte[password.Length];
            hide = Encoding.UTF8.GetBytes(password);
            enteredpassword = Convert.ToBase64String(hide);
            return enteredpassword;
        }
        private string DecryptPassword(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
        //public string Login(UserLoginModel userLogin)
        //{
        //    try
        //    {
        //        var LoginResult = this.fundooContext.UserTable.Where(X => X.Email == userLogin.Email && X.Password == userLogin.Password).FirstOrDefault();
        //        if (LoginResult != null)
        //        {
        //            return LoginResult.Email;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public LoginResponse UserLogin(UserLoginModel userLog)
        {
            try
            {
                var existingLogin = fundooContext.UserTable.Where(X => X.Email == userLog.Email).FirstOrDefault();
                string decryptPass = DecryptPassword(existingLogin.Password);
                if (decryptPass == userLog.Password)
                {
                    LoginResponse login = new LoginResponse();
                    string token = GenerateSecurityToken(existingLogin.Email, existingLogin.Id);
                    //login.Id = existingLogin.Id;
                    //login.FirstName = existingLogin.FirstName;
                    //login.LastName = existingLogin.LastName;
                    login.Email = existingLogin.Email;
                    //login.Password = existingLogin.Password;
                    //login.CreatedAt = existingLogin.CreatedAt;
                    login.Token = token;

                    return login;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GenerateSecurityToken(string Email, long Id)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsettings["Jwt:SecKey"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                new Claim(ClaimTypes.Email,Email),
                new Claim("Id",Id.ToString())
            };
                var token = new JwtSecurityToken(appsettings["Jwt:Issuer"],
                  appsettings["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddMinutes(60),
                  signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public string ForgetPassword(string email)
        {
            try
            {
                var existingLogin = this.fundooContext.UserTable.Where(X => X.Email == email).FirstOrDefault();
                if (existingLogin != null)
                {
                    var token = GenerateSecurityToken(email, existingLogin.Id);
                    new Msmq().SendMessage(token);
                    return token;
                }
                else
                    return null;
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
                if (password.Equals(confirmPassword))
                {
                    UserEntity user = fundooContext.UserTable.Where(e => e.Email == email).FirstOrDefault();
                    user.Password = EncryptPassword(confirmPassword);
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}