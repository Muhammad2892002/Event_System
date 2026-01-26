using Event_System.DbContexts;
using Event_System.Dtos;
using Event_System.Dtos.AttendeeDto;
using Event_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Event_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly EventContext _eventContext;
        public AttendeeController(EventContext eventContext) { 
            _eventContext = eventContext;
        
        }
        [HttpGet("GetByCriteria")]

        public IActionResult GetByCriteria([FromQuery]SearchAttendee searchAttendee) {
            try
            {
                var allAttendes = (from attendee in _eventContext.Attendees
                                   from user in _eventContext.Users.Where(obj=>obj.Id==attendee.UserId) where
                                  (attendee.FirstName.Contains(searchAttendee.AteendeeName) || attendee.LastName.Contains(searchAttendee.AteendeeName) || searchAttendee.AteendeeName == null) &&
                                  (searchAttendee.IsActivated==attendee.IsActive|| searchAttendee.IsActivated==null)&&(
                                  searchAttendee.Role==attendee.Role||searchAttendee.Role==null)
                                  orderby attendee.Id
                                  select new CreateAteendeeDto { 
                                      FirstName=attendee.FirstName,
                                      LastName=attendee.LastName,
                                      Gender=attendee.Gender,
                                      CreatedAt=attendee.CreatedAt,
                                      Email=attendee.Email,
                                      Id=attendee.Id,
                                      IsActive=attendee.IsActive,
                                      UserId=attendee.UserId,
                                      UserName=user.UserName,
                                      Role=attendee.Role,
                                      Password=user.HashedPassword

                                  
                                  
                                  
                                  
                                  
                                  
                                  }).ToList();
                               

                                 



                return Ok(allAttendes);




            }
            catch (Exception message) {
                return BadRequest(message.Message);
            
            
            
            }
        
        
        
        
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody]CreateAteendeeDto createNewAttendee) {
            try {

                User user = new User()
                {
                    Id = 0,
                    UserName = createNewAttendee.UserName,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(createNewAttendee.Password)



                };
               bool IsUserExist = _eventContext.Users.Any(x=>x.UserName==createNewAttendee.UserName);
                if (IsUserExist) {
                    return BadRequest("The User name you entered is exist");
                
                
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
                return Ok("Added Successfully");


            }
            catch (Exception message) { 
               
                return BadRequest(message);
            
            }

         



        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody]Update_Attendee update_Attendee) {
            try {

               var attendee= _eventContext.Attendees.FirstOrDefault(attende => attende.Id == update_Attendee.Id);
                var AttendeeUser = _eventContext.Users.FirstOrDefault(user => user.Id == attendee.UserId);

                attendee.FirstName = update_Attendee.FirstName;
                attendee.LastName = update_Attendee.LastName;
                attendee.Gender = update_Attendee.Gender;
                attendee.CreatedAt = attendee.CreatedAt;
                attendee.Email = update_Attendee.Email;
                attendee.IsActive = update_Attendee.IsActive;
                
                AttendeeUser.UserName = update_Attendee.UserName;
                AttendeeUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(update_Attendee.Password);
                _eventContext.SaveChanges();
                
                return Ok();
            }
            catch(Exception message){ 
                return BadRequest(message);
            
            }
        
        
        
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(long id) {
            try
            {
                var attende = _eventContext.Attendees.FirstOrDefault(attendee => attendee.Id == id);
                if (attende != null)
                {
                    var user = _eventContext.Users.FirstOrDefault(obj => obj.Id == attende.UserId);
                    _eventContext.Attendees.Remove(attende);
                    _eventContext.Users.Remove(user);
                    _eventContext.SaveChanges();
                    return Ok("Attendee Deleted SuccessFully");
                }
                else {

                    return BadRequest("The User Not Found");
                
                }
            }
            catch (Exception message) {


                return BadRequest(message.Message);
            
            }
        }


        

            
       
    }

}
