using Event_System.DbContexts;
using Event_System.Dtos.EventDto;
using Event_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            try {
                var checkOrganizer = eventContext.Attendees.Any(obj=>obj.Id==createEvent.OrganizerId&&obj.Role==(int)Enums.Roles.Organizer);
                if (!checkOrganizer) { 
                
                 return BadRequest("Organizer not found.");
                }
                var checkDateAndTimeEvent = eventContext.Events.Any(obj=>obj.Location==createEvent.Location&&obj.EventDate.Date ==createEvent.EventDate.Date);
                if (checkDateAndTimeEvent) { 
                
                 return BadRequest("Choose another Day or place ");
                }
                if (createEvent.MaxParticipants<=0) { 
                
                  return BadRequest("Please enter a value greater than zero.");
                
                }

                Event addNewEvent=new Event() {
                  Title = createEvent.Title,
                  Description = createEvent.Description,
                  EventDate = createEvent.EventDate,
                  CreatedAt = createEvent.CreatedAt,
                  Location = createEvent.Location,
                  MaxParticipants = createEvent.MaxParticipants,
                  OrganizerId = createEvent.OrganizerId,
                  
                
                
                
                
                
                };
                eventContext.Events.Add(addNewEvent);
                eventContext.SaveChanges();
              

          
                return Ok();
            
            
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
              
                var fetchEvent = eventContext.Events.FirstOrDefault(obj => obj.Id == createEvent.Id);
                if (fetchEvent != null)
                { var checkDateAndTimeEvent = eventContext.Events.Any(obj => obj.Location == createEvent.Location && obj.EventDate.Date == createEvent.EventDate.Date && obj.Id != createEvent.Id);
                    var checkOrganizer = eventContext.Attendees.Any(obj => obj.Id == createEvent.OrganizerId && obj.Role == (int)Enums.Roles.Organizer);
                    if (checkDateAndTimeEvent)
                    {
                        return BadRequest("Choose another Day or place ");




                    }

                    if (createEvent.MaxParticipants <= 0)
                    {

                        return BadRequest("Please enter a value greater than zero.");

                    }

                    if (!checkOrganizer)
                    {

                        return BadRequest("Organizer not found.");
                    }
                   
                   
                  
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
                
                
                 return BadRequest("Event does not exist");
                }

                    return Ok();
            }
            catch (Exception ex) { 
            
               
                return BadRequest(ex.Message);
            
            }
        
        
        
        
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("DeleteEvent")]

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

    }
}
