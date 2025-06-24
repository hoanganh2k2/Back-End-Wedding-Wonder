using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageServiceManager;

        public MessageController(MessageService messageServiceManager)
        {
            _messageServiceManager = messageServiceManager;
        }

        [HttpGet("all")]

        public async Task<ActionResult> GetMessages()
        {
            try
            {
                List<MessageDTO> messages = await _messageServiceManager.GetAllMessagesAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved message information successfully",
                    Data = messages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<ActionResult> GetMessagesByConversationId(int conversationId)
        {
            try
            {
                List<MessageDTO> messages = await _messageServiceManager.GetMessagesByConversationIdAsync(conversationId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved conversation messages successfully",
                    Data = messages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMessageById(int id)
        {
            try
            {
                MessageDTO? message = await _messageServiceManager.GetMessageByIdAsync(id);
                if (message == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Message not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved message information successfully",
                    Data = message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost("start-conversation")]
        public async Task<IActionResult> StartConversation([FromBody] MessageDTO messageDto)
        {
            try
            {
                int conversationId = await _messageServiceManager.StartConversationAsync(messageDto.SenderId, messageDto.ReceiverId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Conversation started successfully",
                    Data = conversationId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMessage([FromBody] MessageDTO messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data"
                });
            }

            try
            {
                if (!string.IsNullOrEmpty(messageDto.AttachmentUrl))
                {
                    var extension = Path.GetExtension(messageDto.AttachmentUrl).ToLower();
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        messageDto.Type = "image";
                    }
                    else if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                    {
                        messageDto.Type = "document";
                    }
                    else
                    {
                        messageDto.Type = "text";
                    }
                }
                var success = await _messageServiceManager.CreateMessageAsync(messageDto);
                if (success)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Message created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Error creating message"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }


        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin, Super Admin")]  
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] MessageDTO messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data"
                });
            }

            try
            {
                bool success = await _messageServiceManager.UpdateMessageAsync(id, messageDto);

                if (success)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Message updated successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Message not found or update failed"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                bool success = await _messageServiceManager.DeleteMessageAsync(id);

                if (success)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Message deleted successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Message not found or deletion failed"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        /*
        [HttpGet("user/conversations")]
        public async Task<ActionResult> GetConversationsForUser()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                List<int> conversationIds = await _messageServiceManager.GetConversationsForUserAsync(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved user conversations successfully",
                    Data = conversationIds
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        */
        [HttpGet("user/receivers")]
        public async Task<ActionResult> GetReceiversForUser()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);  
                List<UserDTO> receivers = await _messageServiceManager.GetReceiversForUserAsync(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved receivers successfully",
                    Data = receivers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchMessages([FromQuery] int conversationId, [FromQuery] string key)
        {
            try
            {
                int userId = UserHelper.GetUserId(User); 
                List<MessageDTO> messages = await _messageServiceManager.SearchMessagesAsync(userId, conversationId, key);

                if (messages == null || messages.Count == 0)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No messages found."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Messages retrieved successfully",
                    Data = messages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
    }
}
