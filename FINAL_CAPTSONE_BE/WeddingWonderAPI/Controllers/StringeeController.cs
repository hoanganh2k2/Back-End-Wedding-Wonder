using Microsoft.AspNetCore.Mvc;
using WeddingWonderAPI.Helper;
using Services;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/stringee")]
    [ApiController]
    public class StringeeController : ControllerBase
    {
        private readonly StringeeTokenService _stringeeTokenService;

        public StringeeController(StringeeTokenService stringeeTokenService)
        {
            _stringeeTokenService = stringeeTokenService;
        }

        [HttpGet("stringee-access-token")]
        public ActionResult GetStringeeAccessToken()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);

                var token = _stringeeTokenService.GenerateAccessToken(userId);

                return Ok(new
                {
                    accessToken = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
