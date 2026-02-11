using Event_System.DbContexts;
using Event_System.Dtos.EventParticipantsDtos;
using Event_System.Dtos.EventRequestDto;
using Event_System.Dtos.EventRequestDTO;
using Event_System.Models;
using Event_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_System.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestToJoinEventController : ControllerBase
    {
        private readonly DbContexts.EventContext _context;

        public RequestToJoinEventController(EventContext context) { 
        
           _context = context;
        
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery]SearchEventRequestsDto obj)   {


            try {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                
                if (role.ToUpper() == "admin".ToUpper())
                {
                    DateTime? requestedDate = obj.RequestedAt?.Date;

                    var allRequests = (from request in _context.EventRequests
                                       join user in _context.Attendees
                                       on request.UserId equals user.Id

                                       where ( obj.UserId == null ||  obj.UserId == request.UserId  ) &&
                                       (   obj.EventId == null || obj.EventId == request.EventId ) &&
                                       (requestedDate == null ||  (request.RequestedAt >= requestedDate && request.RequestedAt < requestedDate.Value.AddDays(1)) ) &&
                                       (  obj.Status == null || obj.Status == request.Status )
                                       select new ShowAllEventRequestsDto
                                       {
                                           Id = request.Id,
                                           UserId = request.UserId,
                                           
                                           EventId = request.EventId,
                                           email = user.Email,
                                           EventName = request.EventNavigation.Title,
                                           Name = user.FirstName + user.LastName,
                                           RequestedAt = request.RequestedAt,
                                           Status = request.Status,
                                           StatusName = Services.getStatusName(request.Status),













                                       }



                                       ).ToList();

                    return Ok(allRequests);

                }
                else
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    int organizerId = int.Parse(userId);
                    DateTime? requestedDate = obj.RequestedAt?.Date;

                    var query =
                        from request in _context.EventRequests
                        join user in _context.Attendees
                            on request.UserId equals user.Id
                        where
                            (obj.UserId == null || obj.UserId == request.UserId) &&
                            request.EventNavigation.OrganizerId == organizerId &&
                            (!requestedDate.HasValue ||
                                (request.RequestedAt >= requestedDate.Value &&
                                 request.RequestedAt < requestedDate.Value.AddDays(1))) &&
                            (obj.Status == null || obj.Status == request.Status)
                        select new ShowAllEventRequestsDto
                        {
                            Id = request.Id,
                            EventId = request.EventId,
                            email = user.Email,
                            EventName = request.EventNavigation.Title,
                            Name = user.FirstName + " " + user.LastName,
                            RequestedAt = request.RequestedAt,
                            Status = request.Status
                        };

                    var allRequests = query.ToList();



                    return Ok(allRequests);




                    return BadRequest();








                }
                return BadRequest();






            }
            catch (Exception ex) { 
            
              return BadRequest(ex.Message);
            
            }
        
        
        }
        //[Authorize(Roles = "Organizer,Admin,Attendee")]
        [HttpPost("CreateRequest")]
        public IActionResult CreateRequest([FromBody]long request) {
            try {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var userApprovedRequestCheck = _context.EventRequests.Any(obj => obj.EventId == request && obj.UserId == userId && obj.Status == (int)Enums.Status.Approved);
                var userPendingRequestCheck = _context.EventRequests.Any(obj => obj.EventId == request && obj.UserId == userId && obj.Status == (int)Enums.Status.pending);
                var isUserExist = _context.Attendees.Any(obj=>obj.Id==userId);
                var isEventExist = _context.Events.Any(obj=>obj.Id== request);
                if (userApprovedRequestCheck) {

                    return BadRequest(new { msg= "You Cannot Join to Event already accepted" });
                }
                if (userPendingRequestCheck)
                {

                    return BadRequest(new { msg = "You cannot send another request." });
                }
                if (!isUserExist && !isEventExist) {
                    return BadRequest("There is no Attendee and Event");
                }
                if (isUserExist && !isEventExist)
                {
                    return BadRequest("There is no  Event");
                }
                if (!isUserExist && isEventExist)
                {
                    return BadRequest("There is no Attendee");
                }
                EventRequest requestEvent= new EventRequest() { 
                
                    EventId = request,
                    UserId = userId,
                    Status=(int)Enums.Status.pending,
                    RequestedAt = DateTime.Now,
                    

                
                
                
                
                };
                _context.EventRequests.Add(requestEvent);
                _context.SaveChanges();


                return Ok(new { msg= "The Requested is send it successfully" });
            }
            catch(Exception ex) {
            
            
              return BadRequest(ex.Message);
            
            }
        
        
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpPut("Update")]
        public IActionResult Update([FromBody]EventRequestDto request)
        {
            try {
                var RequestObj = _context.EventRequests.FirstOrDefault(req => req.Id == request.Id);
                if (RequestObj != null)
                {
                    var isUserExist = _context.Attendees.Any(obj => obj.Id == RequestObj.UserId);
                    var isEventExist = _context.Events.Any(obj => obj.Id == RequestObj.EventId);
                    if (!isUserExist && !isEventExist)
                    {
                        return BadRequest("There is no Attendee and Event");
                    }
                    if (isUserExist && !isEventExist)
                    {
                        return BadRequest("There is no  Event");
                    }
                    if (!isUserExist && isEventExist)
                    {
                        return BadRequest("There is no Attendee");
                    }
                    if (request.Status < (int)Enums.Status.pending || request.Status > (int)Enums.Status.Approved)
                    {

                        return BadRequest("please enter valid status 0=pending 1=rejected 2=approved");
                    }

                    //var requestCheck = _context.EventRequests.FirstOrDefault(obj => obj.Id == request.Id && obj.Status != (int)Enums.Status.Approved);
                    if (RequestObj == null || RequestObj.Status == (int)Enums.Status.Approved)
                    {
                        return BadRequest(new {msg= "This event may be deleted or approved" });

                    }
                    if (request.Status == (int)Enums.Status.Rejected)
                    {

                        RequestObj.Status = (int)Enums.Status.Rejected;
                        _context.SaveChanges();

                        return Ok(new { msg = "Your Request is rejected" });


                    }
                    else if (request.Status == (int)Enums.Status.Approved)
                    {
                        EventParticipant newParticipants = new EventParticipant()
                        {
                            EventId = RequestObj.EventId,
                            UserId = RequestObj.UserId,



                        };
                        _context.EventParticipants.Add(newParticipants);
                        RequestObj.Status = (int)Enums.Status.Approved;




                    }


                    _context.SaveChanges();

                    return Ok(new { msg = "Your Request is approved" });
                }
                return BadRequest();
            }
            catch (Exception ex) {

                return BadRequest();
            }
        
        
        
        
        
        
        }

        //[Authorize(Roles = "Organizer,Admin,Attendee")]
        [HttpDelete("DeleteRequestEvent/{id}")]
        public IActionResult DeleteRequestEvent(long Id) {
            try {
                var Request = _context.EventRequests.FirstOrDefault(obj=>obj.Id==Id);
                if (Request == null) {

                    return BadRequest(new { msg= "This request may be deleted." });
                
                }
                if (Request.Status==(int)Enums.Status.Approved) {
                    var EventParticipants = _context.EventParticipants.FirstOrDefault(obj=>obj.UserId==Request.UserId&&obj.EventId==Request.EventId);
                    if (EventParticipants != null) {


                        _context.EventParticipants.Remove(EventParticipants);
                    
                    }
                
                
                
                }
                _context.EventRequests.Remove(Request);
                _context.SaveChanges();
                return Ok(new { msg= "Deleted Successfully" });
            
            
            }


            catch (Exception ex) {

                return BadRequest();
            
            
            
            }
        
        
        
        }


        [HttpGet("GetAllAttendeesWithRequests")]
        public IActionResult GetAllAttendeesWithRequests() {
            try {
                var allAttendee=_context.EventRequests.GroupBy(obj=>obj.UserId).Select(g=>new GetAllParticipantsDto { 
                  ParticipantUserId=g.Key,
                  ParticipantName=g.First().UserNavigation.FirstName+" "+ g.First().UserNavigation.LastName


                } ).ToList();


                return Ok(allAttendee);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }





            }



        [HttpGet("GetAllEventsWithRequests")]
        public IActionResult GetAllEventsWithRequests()
        {
            try
            {
                var allEvents = _context.EventRequests.GroupBy(obj => obj.EventId).Select(g => new GetAllEventInParticipantDto
                {
                  EventId = g.Key,
                  EventName = g.First().EventNavigation.Title


                }).ToList();


                return Ok(allEvents);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }





        }

        [HttpGet("GetRequestById/{id}")]
        public IActionResult GetRequestById(long id)
        {
            try {
                var getRequest=_context.EventRequests.Where(obj=>obj.Id==id).Select(req=>new ShowAllEventRequestsDto {
                  Id=req.Id,
                  EventId=req.EventId,
                  UserId=req.UserId
                  
                
                
                }).FirstOrDefault();

                return Ok(getRequest);
            }
            catch (Exception ex) { 
            
             return BadRequest(ex.Message);
            
            }
        
        
        }



    }
}
