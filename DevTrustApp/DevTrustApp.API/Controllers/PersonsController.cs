using DevTrustApp.Core.ApplicationService.Interface;
using DevTrustApp.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DevTrustApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;
        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }


        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] string person)
        {
            try
            {
                var result = await _personService.Save(person);
                return Ok(result);
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(GetAllRequest request)
        {
            var result = await _personService.GetAll(request);
            return Ok(result);
        }
    }
}
