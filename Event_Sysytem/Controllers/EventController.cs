using Event_System.DbContexts;
using Event_System.Dtos.EventDto;
using Event_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventContext eventContext;


        public EventController(EventContext _eventContext) {

            eventContext = _eventContext;
        }
        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery]SearchEventDto searchdto) {
            try {
                var allEvents = (from events in eventContext.Events
                             
                                 where (searchdto.Title==events.Title||searchdto.Title==null)&&
                                 (searchdto.OrganizerId==events.OrganizerId||searchdto.OrganizerId==null)&&
                                 (searchdto.CreatedAt==events.CreatedAt||searchdto.CreatedAt==null)&&
                                 (searchdto.EventDate==events.EventDate||searchdto.EventDate==null)&&
                                 (searchdto.LocationId==events.Location|| searchdto.LocationId==null)
                                 select new ShowAllEvents { 
                                     Title=events.Title,
                                     Description=events.Description,
                                     EventDate=events.EventDate,
                                     CreatedAt=events.CreatedAt,
                                     MaxParticipants=events.MaxParticipants,
                                     OrganizerId=events.OrganizerId,
                                     OrganizerName=events.Organizer.FirstName+" "+events.Organizer.LastName,
                                     Location=events.Location,
                                     LocationName=events.LocationNavigation.Name
                                   
                                 
                                 
                                 }
                                 ).ToList();



                return Ok(allEvents);
            
            
            
            
            }
            catch (Exception ex) {
            
            
               
                return BadRequest(ex.Message);
            }
            
        
        
        }
        [HttpPost("CreateEvent")]
        public IActionResult CreateEvent([FromBody] CreateEventDto createEvent)
        {
            try {

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

        [HttpPut("UpdateEvent")]
        public IActionResult UpdateEvent([FromBody] CreateEventDto createEvent)
        {
            try {
                var fetchEvent = eventContext.Events.FirstOrDefault(obj => obj.Id == createEvent.Id);
                if (fetchEvent != null)
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
                
                
                 return BadRequest("Event does not exist");
                }

                    return Ok();
            }
            catch (Exception ex) { 
            
               
                return BadRequest(ex.Message);
            
            }
        
        
        
        
        }

    }
}
