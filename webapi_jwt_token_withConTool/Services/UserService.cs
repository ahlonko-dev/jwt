using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Mapper;
using System.Data.Common;
using ConnectionTool.DataBase;
using System.Data.SqlClient;

namespace WebApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
       
         private List<User> _users = new List<User>();
         private readonly string constring = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = DbUsers; Integrated Security = True;";
         private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
           
           
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            User user = null;

            Connection con = new Connection(SqlClientFactory.Instance, constring);

            Command com = new Command("select Id, email, firstname, lastname from tusers where password='" + model.Password + "'");

            user = con.ExecuteReader(com, u => u.ToApi()).SingleOrDefault();

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            
            //save token in db
            Command comSaveToken = new Command("UPDATE tusers SET Token='" + token.ToString() + "' WHERE Id=" + user.Id);
            con.ExecuteNonQuery(comSaveToken);


            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {

            Connection con = new Connection(SqlClientFactory.Instance, constring);
            Command com = new Command("select Id, email, firstname, lastname, password from tusers");

            return con.ExecuteReader(com, u => u.ToApi());

        }



        public User GetById(int id)
        {
            User u = null;
                     
            Connection con = new Connection(SqlClientFactory.Instance, constring);
            Command com = new Command("select Id, email, firstname, lastname from tusers where id='" + id + "'");

            u = con.ExecuteReader(com, u => u.ToApi()).SingleOrDefault();

            return u;
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}