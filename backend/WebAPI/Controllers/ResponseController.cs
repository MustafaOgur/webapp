using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Business.Abstract;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }


        [HttpPost("AddResponse")]
        public async Task<IActionResult> AddResponseAsync(string messageId)
        {
            var result = await _responseService.AddResponseAsync(messageId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpGet("GetResponseByMessageId")]
        public async Task<IActionResult> GetResponseByMessageIdAsync(string id)
        {
            var result = await _responseService.GetResponseByMessageIdAsync(id);
            
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
