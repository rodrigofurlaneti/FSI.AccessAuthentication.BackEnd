using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static FSI.AccessAuthentication.Api.Controllers.Base.BaseAsyncController;

namespace FSI.AccessAuthentication.Api.Controllers
{
    [ApiController]
    [Route("api/systems/async")]
    public class SystemControllerAsync : BaseAsyncController<SystemDto>
    {
        private readonly ISystemAppService _service;

        public SystemControllerAsync(ISystemAppService service, ILogger<SystemControllerAsync> logger,
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
                    _logger.LogWarning("System with id {SystemId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving traffic with id {SystemId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SystemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for traffic creation: {@SystemDto}", dto);
                    return BadRequest(ModelState);
                }

                await _service.AddAsync(dto);

                _logger.LogInformation("System created with id {SystemId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {@SystemDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] SystemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for user update: {@SystemDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("System ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                var existingSystem = await _service.GetByIdAsync(id);
                if (existingSystem is null)
                {
                    _logger.LogWarning("System with id {SystemId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                _logger.LogInformation("System with id {SystemId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with id {SystemId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var existingSystem = await _service.GetByIdAsync(id);
                if (existingSystem is null)
                {
                    _logger.LogWarning("System with id {SystemId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingSystem.Id);

                _logger.LogInformation("System with id {SystemId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with id {SystemId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            return await SendMessageAsync("getall", new SystemDto(), "POST - MessageGetAll", "user-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            return await SendMessageAsync("getbyid", new SystemDto { Id = id }, "POST - MessageGetById", "user-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] SystemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await SendMessageAsync("create", dto, "POST - MessageCreate", "user-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] SystemDto dto)
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
                    "getall" => JsonSerializer.Deserialize<IEnumerable<SystemDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<SystemDto>(messageResponse),
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

            return await SendMessageAsync("delete", new SystemDto { Id = id }, "DELETE - MessageDelete", "user-queue");
        }

        #endregion
    }
}