using Microsoft.AspNetCore.Mvc;

namespace PatientManagement.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class TextController : ControllerBase
    {

        [HttpGet]
        public IActionResult MyTest()
        {

            return Ok("My test endpoint is fine");
        }
    }
}
