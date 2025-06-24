using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Models.DTOs;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/topic")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly TopicManager _topicManager;

        public TopicController(TopicManager topicManager)
        {
            _topicManager = topicManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllTopics()
        {
            try
            {
                var topics = await _topicManager.GetAllTopicsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get list of topics successfully",
                    Data = topics
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTopicById(int id)
        {
            try
            {
                var topic = await _topicManager.GetTopicByIdAsync(id);
                if (topic == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Topic not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Topic Information Success",
                    Data = topic,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTopic([FromBody] TopicCreateUpdateRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }
            try
            {
                bool status = await _topicManager.CreateTopicAsync(request);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Topic created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to create topic"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] TopicCreateUpdateRequestDTO request)
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
                bool status = await _topicManager.UpdateTopicAsync(id, request);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Topic updated successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Topic not found or update failed"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            try
            {
                bool status = await _topicManager.DeleteTopicAsync(id);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Topic has been deleted"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Topic not found or delete failed"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
    }
}