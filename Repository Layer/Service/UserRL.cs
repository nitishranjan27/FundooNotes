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
                var existLogin = this.fundooContext.UserTable.Where(X => X.Email == userLog.Email && X.Password == userLog.Password).FirstOrDefault();
                if (existLogin != null)
                {
                    LoginResponse login = new LoginResponse();
                    string token = GenerateSecurityToken(existLogin.Email, existLogin.Id);
                    login.Id = existLogin.Id;
                    login.FirstName = existLogin.FirstName;
                    login.LastName = existLogin.LastName;
                    login.Email = existLogin.Email;
                    login.Password = existLogin.Password;
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
    }
}