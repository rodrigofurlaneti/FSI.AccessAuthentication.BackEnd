using FSI.AccessAuthentication.Api.Controllers.Base;
using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.AccessAuthentication.Api.Controllers
{
    [ApiController]
    [Route("api/profiles/async")]
    public class ProfileControllerAsync : BaseAsyncController<ProfileDto>
    {
        private readonly IProfileAppService _service;

        public ProfileControllerAsync(IProfileAppService service, ILogger<ProfileControllerAsync> logger,
            IMessageQueuePublisher publisher, IMessagingAppService messagingService) : base(logger, publisher, messagingService)
        {
            _service = service;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting user");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result is null)
                {
                    _logger.LogWarning("Profile with id {ProfileId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving traffic with id {ProfileId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfileDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for traffic creation: {@ProfileDto}", dto);
                    return BadRequest(ModelState);
                }

                await _service.InsertAsync(dto);

                _logger.LogInformation("Profile created with id {ProfileId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {@ProfileDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] ProfileDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for user update: {@ProfileDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Profile ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                var existingProfile = await _service.GetByIdAsync(id);
                if (existingProfile is null)
                {
                    _logger.LogWarning("Profile with id {ProfileId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                _logger.LogInformation("Profile with id {ProfileId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with id {ProfileId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var existingProfile = await _service.GetByIdAsync(id);
                if (existingProfile is null)
                {
                    _logger.LogWarning("Profile with id {ProfileId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingProfile.Id);

                _logger.LogInformation("Profile with id {ProfileId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with id {ProfileId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            return await SendMessageAsync("getall", new ProfileDto(), "POST - MessageGetAll", "user-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            return await SendMessageAsync("getbyid", new ProfileDto { Id = id }, "POST - MessageGetById", "user-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] ProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await SendMessageAsync("create", dto, "POST - MessageCreate", "user-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] ProfileDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            return await SendMessageAsync("update", dto, "PUT - MessageUpdate", "user-queue");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            return await GetResultAsyncInternal(id, (action, messageResponse) =>
            {
                return action.ToLowerInvariant() switch
                {
                    "getall" => JsonSerializer.Deserialize<IEnumerable<ProfileDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<ProfileDto>(messageResponse),
                    "create" or "update" or "delete" => messageResponse,
                    _ => null
                };
            });
        }

        [HttpDelete("event/delete/{id:long}")]
        public async Task<IActionResult> MessageDeleteAsync(long id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            return await SendMessageAsync("delete", new ProfileDto { Id = id }, "DELETE - MessageDelete", "user-queue");
        }

        #endregion
    }
}