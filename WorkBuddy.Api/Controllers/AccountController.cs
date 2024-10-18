using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WorkBuddy.Api.Data;
using WorkBuddy.Api.DTOs;
using WorkBuddy.Api.Entities;
using WorkBuddy.Api.Interfaces;

namespace WorkBuddy.Api.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService):BaseApiController
    {


        //Uses Http Post from endpoint register , to  register a new user with asyncronous methods 
        //The user data will be handled as a Data Transfer Object , the structure of which is specified in /DTOs directory
        //Using HMACSHA512 to compute hash and using salt for the stored password in DB
        //( i know how to use multiline comments , i just dont like them ^_^ )


        [HttpPost("register")] // account/register

        //Using a DTO , to transfer the register data , so it can be read from any type of request source (eg http body, url parms etc)
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //Check if the user exists
            if (await UserExists(registerDto.UserName,registerDto.Mail))
            {
                return BadRequest("Username Invalid. User already exists");
            }

           

           
            using var hmac = new HMACSHA512();  // For encrypting the passwords

            //Creates the new user so i can add it to context(and then to db)
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PwdHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PwdSalt = hmac.Key,
                Mail = registerDto.Mail,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DepartmentId = registerDto.DepartmentId,
                DateOfBirth = registerDto.DateOfBirth,
                Gender = registerDto.Gender
            };

            //Registers the user to DB
            context.Users.Add(user);
            
            
            await context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Username = registerDto.UserName.ToLower(),
                Token = tokenService.CreateToken(user)
            };
            return userDto;

        }

        [HttpPost("login")]  // account/register

        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //Checks to find the user in the Database
            var user = await context.Users.FirstOrDefaultAsync(x=> x.UserName.ToLower() == loginDto.UserName.ToLower() );

            if (user == null)
            {
                return Unauthorized("Username not Valid");
            }

            using var hmac = new HMACSHA512(user.PwdSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PwdHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            var userDto = new UserDto
            {
                Username = loginDto.UserName.ToLower(),
                Token = tokenService.CreateToken(user)
            };
            return userDto;

            

        }


        private async Task<bool> UserExists(String username, String mail) 
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower() || x.Mail.ToLower() == mail.ToLower());

        }



    }
}
