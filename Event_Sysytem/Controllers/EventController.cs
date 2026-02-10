using Event_System.DbContexts;
using Event_System.Dtos.EventDto;
using  Event_System.Dtos.EventDto.ErrorMassageDto;
using Event_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Event_System.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventContext eventContext;


        public EventController(EventContext _eventContext) {

            eventContext = _eventContext;
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery]SearchEventDto searchdto) {
            try {

                DateTime? eventDate = searchdto.EventDate?.Date;
                DateTime? createdAt = searchdto.CreatedAt?.Date;

                //var role=User.FindFirst(ClaimTypes.Role)?.Value;
                //if (role.ToUpper() == "Admin".ToUpper())
                //{

                    var allEvents = (
         from events in eventContext.Events.AsNoTracking()
         where
             (searchdto.Title == null || events.Title == searchdto.Title)
             &&
             (searchdto.OrganizerId == null || events.OrganizerId == searchdto.OrganizerId)
             &&
             (searchdto.LocationId == null || events.Location == searchdto.LocationId)
             &&
             (
                 eventDate == null ||
                 (events.EventDate >= eventDate &&
                  events.EventDate < eventDate.Value.AddDays(1))
             )
             &&
             (
                 createdAt == null ||
                 (events.CreatedAt >= createdAt &&
                  events.CreatedAt < createdAt.Value.AddDays(1))
             )
         select new ShowAllEvents
         {
             Id = events.Id,
             Title = events.Title,
             Description = events.Description,
             EventDate = events.EventDate,
             CreatedAt = events.CreatedAt,
             MaxParticipants = events.MaxParticipants,
             OrganizerId = events.OrganizerId,
             OrganizerName = events.Organizer.FirstName + " " + events.Organizer.LastName,
             Location = events.Location,
             LocationName = events.LocationNavigation.Name
         }
     ).ToList();
                return Ok(allEvents);



                //}
     //           else { 
     //               var OrganizerId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                
     //                           var allEvents = (
     //    from events in eventContext.Events.AsNoTracking()
     //    where
     //        (searchdto.Title == null || events.Title == searchdto.Title)
     //        &&
     //        (events.OrganizerId == int.Parse(OrganizerId))
     //        &&
     //        (searchdto.LocationId == null || events.Location == searchdto.LocationId)
     //        &&
     //        (
     //            eventDate == null ||
     //            (events.EventDate >= eventDate &&
     //             events.EventDate < eventDate.Value.AddDays(1))
     //        )
     //        &&
     //        (
     //            createdAt == null ||
     //            (events.CreatedAt >= createdAt &&
     //             events.CreatedAt < createdAt.Value.AddDays(1))
     //        )
     //    select new ShowAllEvents
     //    {
     //        Title = events.Title,
     //        Description = events.Description,
     //        EventDate = events.EventDate,
     //        CreatedAt = events.CreatedAt,
     //        MaxParticipants = events.MaxParticipants,
     //        OrganizerId = events.OrganizerId,
     //        OrganizerName = events.Organizer.FirstName + " " + events.Organizer.LastName,
     //        Location = events.Location,
     //        LocationName = events.LocationNavigation.Name
     //    }
     //).ToList();
     //               return Ok(allEvents);
                
                
     //           }


    





                
     




                //return Ok();
            
            
            
            
            }
            catch (Exception ex) {
            
            
               
                return BadRequest(ex.Message);
            }
            
        
        
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpPost("CreateEvent")]
        public IActionResult CreateEvent([FromBody] CreateEventDto createEvent)
        { 
            EventErrorMassageDto error = new EventErrorMassageDto();
            

           
            
            try {
                var checkOrganizer = eventContext.Attendees.Any(obj=>obj.Id==createEvent.OrganizerId&&obj.Role==(int)Enums.Roles.Organizer);
                if (!checkOrganizer) { 
                
                  error.OrganizerError= "Organizer not found.";
                }
                var checkDateAndTimeEvent = eventContext.Events.Any(obj=>obj.Location==createEvent.Location&&obj.EventDate.Date ==createEvent.EventDate.Date);
                if (checkDateAndTimeEvent) { 
                
                 error.ErrorDateErrorMsg="Choose another Day or place ";
                }
                if (createEvent.MaxParticipants<=0) { 
                
                  error.ErrorMassageMaxParticipantsErrorMsg="Please enter a value greater than zero.";
                
                }
                if (error.OrganizerError != null || error.ErrorDateErrorMsg != null || error.ErrorMassageMaxParticipantsErrorMsg != null) { 
                
                  return BadRequest(error);
                }

                Event addNewEvent=new Event() {
                  Title = createEvent.Title,
                  Description = createEvent.Description,
                  EventDate = createEvent.EventDate,
                  CreatedAt = DateTime.Now,
                  Location = createEvent.Location,
                  MaxParticipants = createEvent.MaxParticipants,
                  OrganizerId = createEvent.OrganizerId,
                  
                
                
                
                
                
                };
                eventContext.Events.Add(addNewEvent);
                eventContext.SaveChanges();
              

          
                return Ok(new { massage="Successfully added"});
            
            
            }
            catch (Exception ex) {
                return BadRequest();
            
            
            }
        
        
        }

        //[Authorize(Roles = "Organizer,Admin")]
        [HttpPut("UpdateEvent")]
        public IActionResult UpdateEvent([FromBody] CreateEventDto createEvent)
        {
            try {
                EventErrorMassageDto error = new EventErrorMassageDto();

                var fetchEvent = eventContext.Events.FirstOrDefault(obj => obj.Id == createEvent.Id);
                if (fetchEvent != null)
                { var checkDateAndTimeEvent = eventContext.Events.Any(obj => obj.Location == createEvent.Location && obj.EventDate.Date == createEvent.EventDate.Date && obj.Id != createEvent.Id);
                    var checkOrganizer = eventContext.Attendees.Any(obj => obj.Id == createEvent.OrganizerId && obj.Role == (int)Enums.Roles.Organizer);
                    if (checkDateAndTimeEvent)
                    {
                        error.ErrorDateErrorMsg = "Choose another Day or place ";




                    }

                    if (createEvent.MaxParticipants <= 0)
                    {

                        error.ErrorMassageMaxParticipantsErrorMsg = "Please enter a value greater than zero.";

                    }

                    if (!checkOrganizer)
                    {

                        error.OrganizerError = "Organizer not found.";
                    }

                    if (error.OrganizerError == null && error.ErrorDateErrorMsg == null && error.ErrorMassageMaxParticipantsErrorMsg == null)
                    {






                        fetchEvent.Title = createEvent.Title;
                        fetchEvent.Description = createEvent.Description;
                        fetchEvent.Location = createEvent.Location;
                        fetchEvent.CreatedAt = createEvent.CreatedAt;
                        fetchEvent.EventDate = createEvent.EventDate;
                        fetchEvent.MaxParticipants = createEvent.MaxParticipants;
                        fetchEvent.OrganizerId = createEvent.OrganizerId;

                        eventContext.SaveChanges();
                    }
                    else {

                        return BadRequest(error);
                    }


                }
                else { 
                
                
                 return BadRequest("Event does not exist");
                }

                    return Ok(new { massage="successfully updated"});
            }
            catch (Exception ex) { 
            
               
                return BadRequest(ex.Message);
            
            }
        
        
        
        
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("DeleteEvent/{id}")]

        public IActionResult DeleteEvent(long Id) {
            try {

               var TargtedEvent= eventContext.Events.FirstOrDefault(x=>x.Id==Id);
                var checkAnyParticipants = eventContext.EventParticipants.Any(obj=>obj.EventId==Id);
                if (TargtedEvent != null && !checkAnyParticipants)
                {

                    eventContext.Events.Remove(TargtedEvent);
                    eventContext.SaveChanges();
                    return Ok("Event Deleted Successfully");


                }
                else if (checkAnyParticipants) { 
                
                  return BadRequest("You can’t delete this event while it still has participants.");
                }
                else
                {


                    return BadRequest("This event is deleted or does not exist ");

                }


                    
             
            }
            catch (Exception ex) {

                return BadRequest();
            
            
            }
            



        
        
        
        
        }

        [HttpGet("GetById/{id}")]
        //ShowAllEvents

        public IActionResult GetById(long Id)
        {
            try {
                var getEvent=(from eventObj in eventContext.Events
                             where eventObj.Id==Id
                             select new ShowAllEvents { 
                                 Id= eventObj.Id,
                                 Title= eventObj.Title,
                                 EventDate= eventObj.EventDate,
                                 Location= eventObj.Location,
                                 OrganizerId= eventObj.OrganizerId,
                                 MaxParticipants= eventObj.MaxParticipants,
                                 Description= eventObj.Description,
                                 
                                 
                             
                             }).FirstOrDefault();

                return Ok(getEvent);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            
            
            }

        
        
        
        }

    }
}
