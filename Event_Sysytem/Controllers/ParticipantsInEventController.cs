using Event_System.DbContexts;
using Event_System.Dtos.EventParticipantsDtos;
using Event_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_System.Controllers

{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsInEventController : ControllerBase
    {
        private readonly EventContext _eventContext;
        public ParticipantsInEventController(EventContext eventContext)
        {
            _eventContext = eventContext;
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery]SearchParticipants searchParticipants)
        {
            try
            {
                //var role=User.FindFirst(ClaimTypes.Role)?.Value;
                //var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (role.ToUpper() == "admin".ToUpper())
                //{
                    var allParticipants = (from particinates in _eventContext.EventParticipants
                                           where (searchParticipants.EventId == particinates.EventId || searchParticipants.EventId == null)
                                           && (searchParticipants.ParticipantId == particinates.UserId || searchParticipants.ParticipantId == null)
                                           select new ShowAllParticipantsDto
                                           {
                                               Id=particinates.Id,
                                               LocationId=particinates.EventNavigation.Location,
                                                  OrganizerId=particinates.EventNavigation.OrganizerId,
                                                  
                                               AttendeeId = particinates.UserId,
                                                  EventId = particinates.EventId,
                                               AttendeeName = particinates.UserNavigation.FirstName + particinates.UserNavigation.LastName,
                                               Email = particinates.UserNavigation.Email,
                                               EventName = particinates.EventNavigation.Title,
                                               Location = particinates.EventNavigation.LocationNavigation.Name,
                                               HeldAt = particinates.EventNavigation.EventDate,
                                               OrganizerName = particinates.EventNavigation.Organizer.FirstName + particinates.EventNavigation.Organizer.LastName,




                                           }).ToList();

                    return Ok(allParticipants);

                //}
                //else {

                //    var allParticipants = (from particinates in _eventContext.EventParticipants
                //                           where (particinates.EventNavigation.OrganizerId == int.Parse(userId))
                //                           && (searchParticipants.ParticipantId == particinates.UserId || searchParticipants.ParticipantId == null)
                //                           select new ShowAllParticipantsDto
                //                           {
                //                               AttendeeName = particinates.UserNavigation.FirstName + particinates.UserNavigation.LastName,
                //                               Email = particinates.UserNavigation.Email,
                //                               EventName = particinates.EventNavigation.Title,
                //                               Location = particinates.EventNavigation.LocationNavigation.Name,
                //                               HeldAt = particinates.EventNavigation.EventDate,
                //                               OrganizerName = particinates.EventNavigation.Organizer.FirstName + particinates.EventNavigation.Organizer.LastName,




                //                           }).ToList();

                //    return Ok(allParticipants);

                //}
                //return BadRequest();
             


               
            }
            catch (Exception ex) { 
            
            
             return BadRequest(ex.Message);
            
            }
        
        
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpPost("AddNewParticipants")]
        public IActionResult AddNewParticipants(AddParticipantsDto addparticipantsDto)
        {
            try {
                var isUserExist=_eventContext.Attendees.Any(obj=>obj.Id==addparticipantsDto.ParticipantId);
                var isEventExist = _eventContext.Events.Any(obj=>obj.Id==addparticipantsDto.EventId);
                var isUserExistInParticipant = _eventContext.EventParticipants.Any(obj=>obj.UserId==addparticipantsDto.ParticipantId&&obj.EventId==addparticipantsDto.EventId);
                if (isUserExist && isEventExist && !isUserExistInParticipant)
                {
                    EventParticipant newParticipants = new EventParticipant()
                    {
                        UserId = addparticipantsDto.ParticipantId,
                        EventId = addparticipantsDto.EventId,

                    };
                    EventRequest request = new EventRequest()
                    {
                        EventId = addparticipantsDto.EventId,
                        UserId = addparticipantsDto.ParticipantId,
                        Status = (int)Enums.Status.Approved,
                        RequestedAt = DateTime.Now,



                    };
                    _eventContext.EventRequests.Add(request);
                    _eventContext.EventParticipants.Add(newParticipants);
                    _eventContext.SaveChanges();

                    return Ok("Added successfully");

                }
                else if (isUserExist && isEventExist && isUserExistInParticipant)
                {

                    return BadRequest("This User is already participants");


                }
                else {
                    if (!isUserExist) {

                        return BadRequest("The user does not exist");
                    }
                    if (!isEventExist) { 
                    
                      return BadRequest("NO Event found!!!!");
                    }
                    return BadRequest();

                    
                }

              
            
            } 
            
            
            
            catch (Exception ex) { 
            
              
                return BadRequest(ex.Message);
            
            }
        
        
        
        
        }

        //[Authorize(Roles = "Organizer,Admin")]
        [HttpPut("EditRequest")]
        public IActionResult EditRequest(EditParicipants editParticipantsDto)
        {
            try {
                var participantCheck = _eventContext.EventParticipants.Any(obj=>obj.UserId==editParticipantsDto.ParticipantId&&obj.EventId==editParticipantsDto.EventId);
                if (participantCheck) {

                    return BadRequest("Cannot Add Participant already exist within same event");
                
                }
                var checkTheParticipantId = _eventContext.EventParticipants.Any(obj=>obj.Id==editParticipantsDto.Id);
                if (!checkTheParticipantId) {
                    return BadRequest("The Id you entered does not exist");


                }
                var isUserExist = _eventContext.Attendees.Any(obj => obj.Id == editParticipantsDto.ParticipantId);
                var isEventExist = _eventContext.Events.Any(obj => obj.Id == editParticipantsDto.EventId);
                var participant = _eventContext.EventParticipants.FirstOrDefault(obj=>obj.Id==editParticipantsDto.Id);
                if (participant != null && isUserExist && isEventExist) { 
                  var RequestEvent = _eventContext.EventRequests.FirstOrDefault(obj => obj.UserId == participant.UserId &&
                obj.EventId == participant.EventId && obj.Status == (int)Enums.Status.Approved
                );
                    if (RequestEvent != null)
                    {
                        participant.UserId = editParticipantsDto.ParticipantId;
                        participant.EventId = editParticipantsDto.EventId;

                        RequestEvent.UserId = participant.UserId;
                        RequestEvent.EventId = participant.EventId;
                        RequestEvent.RequestedAt = DateTime.Now;

                        _eventContext.SaveChanges();
                        return Ok("Updated Successfully");




                    }
                    else {
                       

                        return BadRequest("The Request Does not exist");
                    
                    
                    }
                
                
                }
                if (!isUserExist && !isEventExist)
                {

                    return BadRequest("The User and Event Does not exist");

                }
                if (!isUserExist && isEventExist)
                {

                    return BadRequest("The user does not exist");
                }
                if (isUserExist && !isEventExist)
                {

                    return BadRequest("The Event does not exist");
                }




                return Ok("Edited successfully");
            
            
            
            } 
            
            catch (Exception ex) {


                return BadRequest();
            }
        
        
        
        
        
        
        
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("DeleteParticpants")]

        public IActionResult DeleteRequest(int id) {
            try {
                var participant = _eventContext.EventParticipants.FirstOrDefault(obj => obj.Id ==id);
                if (participant != null)
                {
                    var RequestEvent = _eventContext.EventRequests.FirstOrDefault(obj => obj.UserId == participant.UserId &&
             obj.EventId == participant.EventId && obj.Status == (int)Enums.Status.Approved
             );
                    if (RequestEvent != null)
                    {
                        RequestEvent.Status = (int)Enums.Status.Rejected;
                        _eventContext.EventParticipants.Remove(participant);
                        _eventContext.SaveChanges();

                    }






                }

                else {

                    return BadRequest("The Id you entered does not exist");
                
                }

                    return Ok("DeletedSuccessfully");
            } 
            
            catch (Exception ex) {

                return BadRequest();
            
            
            
            
            }
        
        
        }
        [HttpGet("AllParticipantsInEvent")]

        public IActionResult AllParticipantsInEvent()
        {
            try
            {
                var allParticipants = _eventContext.EventParticipants
       .GroupBy(p => p.UserId)   // group by UserId to remove duplicates
       .Select(g => new GetAllParticipantsDto
       {
           ParticipantUserId = g.Key,
           ParticipantName = g.First().UserNavigation.FirstName + " " + g.First().UserNavigation.LastName
       })
       .ToList();

                return Ok(allParticipants);



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);



            }
        }


        [HttpGet("AllEventsInParticipant")]

        public IActionResult AllEventsInParticipant()
        {
            try
            {
                var allEventsInParticipants = _eventContext.EventParticipants.GroupBy(p => p.EventId).Select(
                    
                    obj=>new GetAllEventInParticipantDto
                    {
                        EventId = obj.First().EventId,
                        EventName = obj.First().EventNavigation.Title
                    }
                    ).ToList();
                return Ok(allEventsInParticipants);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);



            }
        }
    }
}
