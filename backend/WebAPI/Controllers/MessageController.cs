using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Business.Abstract;
using Model.DTOs.Message;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }


        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessageAsync([FromBody] CreateMessageDto createMessageDto)
        {
            var result = await _messageService.AddMessageAsync(createMessageDto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("GetMessagesByChatId")]
        public async Task<IActionResult> GetMessagesByChatIdAsync(string id)
        {
            var result = await _messageService.GetMessagesByChatIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }


        [HttpDelete("DeleteMessage")]
        public async Task<IActionResult> DeleteMessageAsync(string id)
        {
            var result = await _messageService.DeleteMessageAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}