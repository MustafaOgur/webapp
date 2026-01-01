using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.Chat;
using System;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }


        [HttpPost("CreateChat")]
        public async Task<IActionResult> CreateChatAsync([FromBody] CreateChatDto createChatDto)
        {

            var result = await _chatService.CreateChatAsync(createChatDto);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("GetAllChats")]
        public async Task<IActionResult> GetAllChatsAsync()
        {
            var result = await _chatService.GetAllChatsAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }


        [HttpGet("GetChatById")]
        public async Task<IActionResult> GetChatByIdAsync(string id)
        {

            var result = await _chatService.GetChatByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }


        [HttpPost("UpdateChat")]
        public async Task<IActionResult> UpdateChatAsync([FromBody] UpdateChatDto updateChatDto)
        {
            var result = await _chatService.UpdateChatAsync(updateChatDto.Id, updateChatDto);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }



        [HttpDelete("DeleteChat")]
        public async Task<IActionResult> DeleteChatAsync(string id)
        {

            var result = await _chatService.DeleteChatAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }


        [HttpGet("GetChatHistory")]
        public async Task<IActionResult> GetChatHistoryAsync(string chatId)
        {
            var result = await _chatService.GetChatHistoryAsync(chatId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
