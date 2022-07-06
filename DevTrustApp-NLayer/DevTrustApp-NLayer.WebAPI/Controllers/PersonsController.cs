using DevTrustApp_NLayer.WebAPI.BL.Interfaces;
using DevTrustApp_NLayer.WebAPI.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrustApp_NLayer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonBL _personBL;
        public PersonsController(IPersonBL personBL)
        {
            _personBL = personBL;
        }


        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] string person)
        {
            try
            {
                var result = await _personBL.Save(person);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllRequest request)
        {
            var result = await _personBL.GetAll(request);
            return Ok(result);
        }
    }
}
