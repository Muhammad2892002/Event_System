using Event_System.DbContexts;
using Event_System.Dtos;
using Event_System.Dtos.AttendeeDto;
using Event_System.Dtos.AttendeeDto.MassageErrorDto;
using Event_System.Dtos.EventDto;
using Event_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Event_System.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly EventContext _eventContext;
        public AttendeeController(EventContext eventContext)
        {
            _eventContext = eventContext;

        }
        
        [HttpGet("GetByCriteria")]

        public IActionResult GetByCriteria([FromQuery] SearchAttendee? searchAttendee)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value.ToUpper();
               
                if (role == "admin".ToUpper())
                {

                    var allAttendes = (from attendee in _eventContext.Attendees
                                       join user in _eventContext.Users
                                       on attendee.UserId equals user.Id

                                       where
                                  (

                                     (searchAttendee.AteendeeFirstName == null || attendee.FirstName.ToUpper().Trim().Contains(searchAttendee.AteendeeFirstName.ToUpper().Trim()))
                                     &&
                                      (searchAttendee.AteendeeLastName == null || attendee.LastName.ToUpper().Trim().Contains(searchAttendee.AteendeeLastName.ToUpper().Trim()))
                                      &&

                                    (searchAttendee.IsActivated == attendee.IsActive || searchAttendee.IsActivated == null)
                                    &&
                                    (attendee.Gender == searchAttendee.Gender || searchAttendee.Gender == null) &&


                                    (attendee.Role == (int)searchAttendee.Role || searchAttendee.Role == null)
                                   )
                                       orderby attendee.Id
                                       select new ShowAllAtendeeDto
                                       {
                                           Id = attendee.Id,
                                           UserId = attendee.UserId,
                                           Role = attendee.Role,
                                           FirstName = attendee.FirstName,
                                           LastName = attendee.LastName,
                                           Gender = attendee.Gender,

                                           GenderName = attendee.Gender ? "Male" : "Female",
                                           CreatedAt = attendee.CreatedAt,
                                           Email = attendee.Email,

                                           IsActive = attendee.IsActive,

                                           UserName = user.UserName,

                                           Password = user.HashedPassword,
                                           RoleName = attendee.RoleNavigation.Name









                                       }).ToList();

                    return Ok(allAttendes);



                }
                else {
                    var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;



                    var allAttendes = (from attendee in _eventContext.Attendees
                                           join user in _eventContext.Users
                                           on attendee.UserId equals user.Id

                                           where
                                      (
                                       (searchAttendee.AteendeeFirstName == null || attendee.FirstName.ToUpper().Trim().Contains(searchAttendee.AteendeeFirstName.ToUpper().Trim()))
                                     &&
                                      (searchAttendee.AteendeeLastName == null || attendee.LastName.ToUpper().Trim().Contains(searchAttendee.AteendeeLastName.ToUpper().Trim()))
                                      &&

                                    (searchAttendee.IsActivated == attendee.IsActive || searchAttendee.IsActivated == null)
                                    &&
                                    (attendee.Gender == searchAttendee.Gender || searchAttendee.Gender == null) &&


                                    (attendee.Role == (int)searchAttendee.Role || searchAttendee.Role == null)&&

                                        attendee.Id==long.Parse(UserId)
                                       )
                                           orderby attendee.Id
                                           select new ShowAllAtendeeDto
                                           {
                                               Id = attendee.Id,
                                               UserId = attendee.UserId,
                                               Role = attendee.Role,
                                               FirstName = attendee.FirstName,
                                               LastName = attendee.LastName,
                                               Gender = attendee.Gender,

                                               GenderName = attendee.Gender ? "Male" : "Female",
                                               CreatedAt = attendee.CreatedAt,
                                               Email = attendee.Email,

                                               IsActive = attendee.IsActive,

                                               UserName = user.UserName,

                                               Password = user.HashedPassword,
                                               RoleName = attendee.RoleNavigation.Name









                                           }).ToList();







                    
                
                 return Ok(allAttendes);
                
                }






                   




            }
            catch (Exception message)
            {
                return BadRequest(message.Message);



            }




        }
        
        [HttpPost("Create")]

        public IActionResult Create([FromBody] CreateAteendeeDto createNewAttendee)
        {
            ErrorMassageDto msg= new ErrorMassageDto();
            User user=new User();
            try
            {
                bool IsUserNameExist = _eventContext.Users.Any(x => x.UserName == createNewAttendee.UserName);
                if (IsUserNameExist)
                {
                    msg.UserName="*The user name is Exist"; 



                }
          
                bool isEmailExist = _eventContext.Attendees.Any(x => x.Email == createNewAttendee.Email);
                bool doesNameExist = _eventContext.Attendees.Any(x => x.FirstName == createNewAttendee.FirstName && x.LastName == createNewAttendee.LastName);
                if (doesNameExist)
                {

                    msg.FullName = "*The Name you entered exist";


                }
                if (isEmailExist)
                {

                   
                    msg.Email="*The Email you entered is exist";
                }






                if (msg.UserName==null&&msg.FullName==null&&msg.Email==null)
                {
                    user = new User()
                    {
                        Id = 0,
                        UserName = createNewAttendee.UserName,
                        HashedPassword = BCrypt.Net.BCrypt.HashPassword(createNewAttendee.Password)



                    };
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
                        Role = createNewAttendee.Role??4,







                    };
                    _eventContext.Attendees.Add(attendee);
                    _eventContext.SaveChanges();
                    return Ok(new {massage="Added successfully" });

                }

                else { 
                
                  
                    return BadRequest(msg);
                
                }
                


            }
            catch (Exception message)
            {

                return BadRequest(message);

            }





        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]

        public IActionResult Update([FromBody] Update_Attendee update_Attendee)
        {
            try
            {
                ErrorMassageDto msg = new ErrorMassageDto();

                var attendee = _eventContext.Attendees.FirstOrDefault(attende => attende.Id == update_Attendee.Id);
                if (attendee != null)
                {
                    var attendeeUser = _eventContext.Users.FirstOrDefault(user => user.Id == attendee.UserId);
                    var isEmailExist = _eventContext.Attendees.Any(obj => obj.Email == update_Attendee.Email && obj.Id != attendee.Id);
                    var isUserNameExist = _eventContext.Users.Any(obj => obj.UserName == update_Attendee.UserName && obj.Id != attendeeUser.Id);
                    if (isUserNameExist)
                    {
                        msg.UserName = "You cant use Existed User Name";

                    }
                    var checkNameExist = _eventContext.Attendees.Any(obj => obj.Id != update_Attendee.Id && (obj.FirstName == update_Attendee.FirstName && obj.LastName == update_Attendee.LastName));
                    if (checkNameExist)
                    {

                        msg.FullName = "This name is already exist.";
                    }

                    if (isEmailExist)
                    {

                        msg.Email = "You cant Use This Email";


                    }


                    if (msg.UserName == null && msg.FullName == null && msg.Email == null)
                    {
                        attendee.FirstName = update_Attendee.FirstName;
                        attendee.LastName = update_Attendee.LastName;
                        attendee.Gender = update_Attendee.Gender;
                        attendee.CreatedAt = attendee.CreatedAt;
                        attendee.Email = update_Attendee.Email;
                        attendee.IsActive = update_Attendee.IsActive;
                        attendee.Role = update_Attendee.Role ?? 4;

                        attendeeUser.UserName = update_Attendee.UserName;
                        if (update_Attendee.Password != null)
                        {
                            attendeeUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(update_Attendee.Password);
                        }
                        _eventContext.SaveChanges();

                        return Ok(new { masg = "Updated Successfully" });
                    }
                    else {

                        return BadRequest(msg);
                    
                    }
                }
                return BadRequest("There is No Attendee");



            }
            catch (Exception message)
            {
                return BadRequest(message);

            }



        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(long? id)
        {
            try

            {
                string massage = string.Empty;
                var attende = _eventContext.Attendees.FirstOrDefault(attendee => attendee.Id == id);

                if (attende != null)
                {
                    if (attende.Role == (int)Enums.Roles.Organizer)
                    {

                        var CheckEventOrganize = _eventContext.Events.Any(obj => obj.OrganizerId == attende.Id);
                        if (CheckEventOrganize)
                        {

                            return BadRequest( new {msg= "This organizer cannot be deleted. Please remove all attendees from all events organized by this user first." });
                        }
                        massage = "Organizer";







                    }
                    else if (attende.Role == (int)Enums.Roles.Attendee)
                    {
                        var checkIfHeIsParticipants = _eventContext.EventParticipants.Any(obj => obj.UserId == attende.Id);

                        if (checkIfHeIsParticipants)
                        {

                            return BadRequest(new { msg = "You cannot delete this attendee while he is  registered as a participant in one or more events." });

                        }
                        massage = "Attendee";




                    }

                    var user = _eventContext.Users.FirstOrDefault(obj => obj.Id == attende.UserId);
                    var allRequestsToBeDeleted = _eventContext.EventRequests.Where(req => req.UserId == attende.Id).ToList();
                    _eventContext.EventRequests.RemoveRange(allRequestsToBeDeleted);
                    _eventContext.Users.Remove(user);
                    _eventContext.Attendees.Remove(attende);

                    _eventContext.SaveChanges();
                    return Ok(new { msg= $"Removed{massage}successfully" });
                }
                else
                {

                    return BadRequest("The User Not Found");

                }
            }
            catch (Exception message)
            {


                return BadRequest(message.Message);

            }
        }

        [HttpGet("GetAllOrganizers")]
        public IActionResult GetAllOrganizers()
        {
            try
            {
                var allOrganizers = (from organizers in _eventContext.Attendees
                                     where organizers.Role == (int)Enums.Roles.Organizer
                                     select new GetAllOrganizersDto
                                     {
                                         Id = organizers.Id,
                                         Name = organizers.FirstName + " " + organizers.LastName

                                     }).ToList();
                return Ok(allOrganizers);


            }
            catch (Exception message)
            {
                return BadRequest(message.Message);


            }







        }

        [HttpGet("GetAttendeeById/{id}")]

        public IActionResult GetAttendeeById(long id) {
            try {
                var GetAttendee=(from attendee in _eventContext.Attendees
                                 where attendee.Id==id select new ShowAllAtendeeDto { 
                                   Id = attendee.Id,
                                   FirstName = attendee.FirstName,
                                   LastName = attendee.LastName,
                                   Email=attendee.Email,
                                   Gender=attendee.Gender,
                                   UserId=attendee.UserId,
                                   UserName=attendee.UserNavigation.UserName,
                                   Role=attendee.Role,
                                   IsActive=attendee.IsActive
                                   
                                   
                                   
                                 
                                   
                                   
                                 
                                 
                                 } ).FirstOrDefault();
                return Ok(GetAttendee);
            
            }
            catch (Exception message) { 
            
             return BadRequest(message.Message);
            }
        
        
        
        }

    }
}
