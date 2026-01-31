using Event_System.DbContexts;
using Event_System.Dtos.LookUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event_System.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly EventContext _context;

        public LookupsController(EventContext context) { 
        
         _context = context;
        }
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpGet("FetchAllRoles")]
        public IActionResult FetchAllRoles()
        {
            try {
                var MajorCode = 0;
               var FetchAllRoles=(from role in _context.Lookups
                                  where(role.MajorCode==MajorCode && (role.MinorCode!=1&&role.MinorCode!=0))
                                  select new LookUpDto { 
                                   Id = role.Id,
                                   Name = role.Name,
                                  
                                  }).ToList();

                return Ok(FetchAllRoles);
            }
            catch(Exception ex) {
                return BadRequest(ex.ToString());
            
            }
        

        
        }


        //[Authorize(Roles = "Organizer,Admin")]
        [HttpGet("GetAllPlaces")]
        public IActionResult GetAllPlaces()
        {
            try
            {
                var MajorCode = 1;
                var FetchAllPlaces = (from place in _context.Lookups
                                     where (place.MajorCode == MajorCode && (place.MinorCode != 1))
                                     select new LookUpDto
                                     {
                                         Id = place.Id,
                                         Name = place.Name,

                                     }).ToList();

                return Ok(FetchAllPlaces);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }



        }



    }
}
