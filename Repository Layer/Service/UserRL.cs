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
        private readonly FundooContext fundooContext;  //context class is used to query or save data to the database.
        private readonly IConfiguration appsettings;   //IConfiguration interface is used to read Settings and Connection Strings from AppSettings.
        public UserRL(FundooContext fundooContext, IConfiguration Appsettings)
        {
            this.fundooContext = fundooContext;
            this.appsettings = Appsettings;
        }
        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="userRegModel">userRegModel Parameter</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for EncryptPassword
        /// </summary>
        /// <param name="password">password Parameter</param>
        /// <returns></returns>
        private string EncryptPassword(string password)
        {
            string enteredpassword = "";
            byte[] hide = new byte[password.Length];
            hide = Encoding.UTF8.GetBytes(password);
            enteredpassword = Convert.ToBase64String(hide);
            return enteredpassword;
        }
        /// <summary>
        /// Method for DecryptPassword
        /// </summary>
        /// <param name="encryptpwd">encryptpwd Parameter</param>
        /// <returns></returns>
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

        /// <summary>
        /// Show All Registerd Login Data
        /// </summary>
        /// <param name="userLog">userLog Parameter</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for Generating Security Token
        /// </summary>
        /// <param name="Email">Email Parameter</param>
        /// <param name="Id">Id Parameter</param>
        /// <returns></returns>
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
        /// <summary>
        /// Method for forget password
        /// </summary>
        /// <param name="email">email Parameter</param>
        /// <returns></returns>
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
        /// <summary>
        /// Method for Reset password
        /// </summary>
        /// <param name="email">email parameter</param>
        /// <param name="password">password parameter</param>
        /// <param name="confirmPassword">confirmPassword parameter</param>
        /// <returns></returns>
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