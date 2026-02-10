using Event_System.DbContexts;
using Event_System.Dtos;
using Event_System.Dtos.Login;
using Event_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Event_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EventContext _eventContext;
        public AuthController(EventContext eventContext) {
            _eventContext = eventContext;
        
        
        
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            try {
                long? attendeeId = 0;
                var checkUser = _eventContext.Users.FirstOrDefault(user => user.UserName == loginDto.Username);
                if (checkUser != null)
                {
                    bool checkPass = BCrypt.Net.BCrypt.Verify(loginDto.Password, checkUser.HashedPassword);
                    if (checkPass) {
                        if (!checkUser.IsAdmin) { 
                        
                        var attendee= _eventContext.Attendees.FirstOrDefault(obj => obj.UserId == checkUser.Id);
                            attendeeId = attendee.Id;
                         
                        }
                         var token = GenerateJwtToken(checkUser,attendeeId);

                       
                        return Ok(token);
                       
                    
                    }
                    else {

                        return BadRequest("Wrong password");
                    }

                }
                else {

                    return BadRequest(new { msg= "User not found" });
                }

                return BadRequest();
            } 
            
            catch(Exception ex) {
                return BadRequest(ex.Message);
            
             
            
            }
        
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] CreateAteendeeDto createNewAttendee) {

            try
            {
                bool IsUserNameExist = _eventContext.Users.Any(x => x.UserName == createNewAttendee.UserName);
                if (IsUserNameExist)
                {
                    return BadRequest("The user name is Exist");



                }


                User user = new User()
                {
                    Id = 0,
                    UserName = createNewAttendee.UserName,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(createNewAttendee.Password)



                };
                bool isEmailExist = _eventContext.Attendees.Any(x => x.Email == createNewAttendee.Email);
                bool doesNameExist = _eventContext.Attendees.Any(x => x.FirstName == createNewAttendee.FirstName && x.LastName == createNewAttendee.LastName);
                if (doesNameExist)
                {
                    return BadRequest("The Name you entered exist");


                }
                if (isEmailExist)
                {

                    return BadRequest("The Email you entered is exist");
                }


                _eventContext.Users.Add(user);





                Attendee attendee = new Attendee()
                {
                    FirstName = createNewAttendee.FirstName,
                    LastName = createNewAttendee.LastName,
                    Email = createNewAttendee.Email,
                    Gender = createNewAttendee.Gender,
                    IsActive = createNewAttendee.IsActive,
                    UserId = user.Id,
                    UserNavigation = user,
                    CreatedAt = DateTime.Now,
                    Role = 4,







                };
                _eventContext.Attendees.Add(attendee);
                _eventContext.SaveChanges();
                return Ok("Registered Successfully");


            }
            catch (Exception message)
            {

                return BadRequest(message);

            }



        }


        private TokenDto GenerateJwtToken(User user,long? attendeeId) {

            var claims = new List<Claim>();
            if (!user.IsAdmin) { 

             claims.Add(new Claim(ClaimTypes.NameIdentifier, attendeeId.ToString()));
            
            }
           
            claims.Add(new Claim(ClaimTypes.Name,user.UserName));
            string role="";
            if (user.IsAdmin)
            {

                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                role= "Admin";

            }
            else {
                var Attendee = _eventContext.Attendees.FirstOrDefault(obj=>obj.Id==attendeeId);
                if (Attendee.Role == (int)Enums.Roles.Attendee)
                {

                    claims.Add(new Claim(ClaimTypes.Role, "Attendee"));
                    role = "Attendee";



                }
                else if (Attendee.Role == (int)Enums.Roles.Organizer) {
                    claims.Add(new Claim(ClaimTypes.Role, "Organizer"));
                    role = "Organizer";



                }
            
            
            
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenSettings = new JwtSecurityToken(
              claims: claims,
             signingCredentials: creds,
             expires: DateTime.Now.AddDays(1)





              );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenSettings);
            return new TokenDto { Token = token, Role = role };




        }



       
    }
}
