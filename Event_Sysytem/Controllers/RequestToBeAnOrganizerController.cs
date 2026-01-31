using Event_System.DbContexts;
using Event_System.Dtos.OrganizerRequest;
using Event_System.Dtos.OrganizerRequestDto;
using Event_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Event_System.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestToBeAnOrganizerController : ControllerBase
    {
        private readonly EventContext _eventContext;
        public RequestToBeAnOrganizerController(EventContext eventContext) { 
        
        
           _eventContext = eventContext;
        
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetByCriteria")]
        public IActionResult GetByCriteria([FromQuery]SearchRequestDto searchRequestDto) {
            try
            {
                DateTime? requestedAt=searchRequestDto.RequestedTime?.Date;
            

                var allRequests = (from requests in _eventContext.OrganizerRequests
                                   where (requests.Status==searchRequestDto.Status||searchRequestDto.Status==null)&&
                                   (searchRequestDto.RequestedTime==null ||(requests.RequestedAt>= requestedAt && requests.RequestedAt<requestedAt.Value.AddDays(1)))
                                   select new ShowOrganizerRequest { 
                                       UserId=requests.UserId,
                                       RequestedAt=requests.RequestedAt,
                                       Status=requests.Status,
                                       Name=requests.UserNavigation.FirstName+" "+requests.UserNavigation.LastName,
                                       StatusNamer=Service.Services.getStatusName(requests.Status)
                                   
                                   
                                   
                                   
                                   
                                   
                                   }).ToList();
                                
                return Ok(allRequests);



            }
            catch (Exception ex) {
            
              return BadRequest(ex.Message);
            }
        
           
        
        }
        //[Authorize(Roles = "Admin,Attendee")]
        [HttpPost("SendRequest")]
        public IActionResult SendRequest(int AttendeeId)
        {
          
            try {
                var checkAttendee= _eventContext.Attendees.Any(x=>x.Id==AttendeeId);
                if (checkAttendee)
                {
                    OrganizerRequest request = new OrganizerRequest()
                    {
                        UserId = AttendeeId,
                        Status = (int)Enums.Status.pending,
                        RequestedAt = DateTime.Now,





                    };
                    _eventContext.OrganizerRequests.Add(request);
                    _eventContext.SaveChanges();



                    return Ok("Request Sended SuccessFully");
                }
                else {
                    return BadRequest("The User You entered does not exist ");
                
                
                }
              
            }
            catch (Exception ex) { 
            
               
                return BadRequest(ex.Message);
            
            
            }
        
        
        
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("UpdateRequest")]
        public IActionResult UpdateRequest(UpdateOrganizerRequestDto obj)
        {
            try {
                 var isRequestExist = _eventContext.OrganizerRequests.Any(x=>x.Id==obj.RequestId);
                if (isRequestExist)
                {
                    var Request = _eventContext.OrganizerRequests.FirstOrDefault(x => x.Id == obj.RequestId);
                    if (obj.Status == 1)
                    {
                        Request.Status = (int)Enums.Status.Rejected;
                        _eventContext.SaveChanges();




                    }
                    else if (obj.Status == 2)
                    {

                        var attendee = _eventContext.Attendees.FirstOrDefault(x => x.Id == Request.UserId);
                        if (attendee != null)
                        {
                            Request.Status = (int)Enums.Status.Approved;

                            attendee.Role = 3;
                            _eventContext.SaveChanges();


                        }
                        else
                        {
                            return BadRequest("Attendee not found!");


                        }



                    }
                    else if (obj.Status < (int)Enums.Status.pending || obj.Status > (int)Enums.Status.Approved) {

                        return BadRequest("You entered an invalid status. You can use: rejected = 1 or approved = 2");
                    
                    }


                        return Ok("Request Added Successfully");
                }
                else { 
                
                 return BadRequest("Request do not exist ");
                
                }

               
               
            
            
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            
            
            }
        
        
        
        }
        //[Authorize(Roles = "Admin,Attendee")]
        [HttpDelete("DeleteRequest")]
        public IActionResult Deleterequest(long Id) {
            try
            {
                var OrganizerRequest = _eventContext.OrganizerRequests.FirstOrDefault(obj => obj.Id == Id);
                

                if (OrganizerRequest != null) {
                    if (OrganizerRequest.Status == (int)Enums.Status.Rejected)
                    {
                        _eventContext.OrganizerRequests.Remove(OrganizerRequest);
                        _eventContext.SaveChanges();
                        return Ok("Deleted SuccessFully");



                    }
                    else {
                        return BadRequest("You cant Delete Accepted or Pending Request");
                    
                    
                    
                    }
                
                
                
                }
                return BadRequest("The Request is Deleted or does not exist");
               

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            
            
            
            
            }
        
        
        
        
        }
    }
}
