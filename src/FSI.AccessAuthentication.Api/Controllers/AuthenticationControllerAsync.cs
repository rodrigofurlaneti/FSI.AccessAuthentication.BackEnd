using FSI.AccessAuthentication.Api.Controllers.Base;
using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static FSI.AccessAuthentication.Api.Controllers.Base.BaseAsyncController;

namespace FSI.AccessAuthentication.Api.Controllers
{
    [ApiController]
    [Route("api/authetications/async")]
    public class AutheticationControllerAsync : BaseAsyncController<AutheticationDto>
    {
        private readonly IAutheticationAppService _service;

        public AutheticationControllerAsync(IAutheticationAppService service, ILogger<AutheticationControllerAsync> logger,
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
                    _logger.LogWarning("Authetication with id {AutheticationId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving traffic with id {AutheticationId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AutheticationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for traffic creation: {@AutheticationDto}", dto);
                    return BadRequest(ModelState);
                }

                await _service.AddAsync(dto);

                _logger.LogInformation("Authetication created with id {AutheticationId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {@AutheticationDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] AutheticationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for user update: {@AutheticationDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Authetication ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                var existingAuthetication = await _service.GetByIdAsync(id);
                if (existingAuthetication is null)
                {
                    _logger.LogWarning("Authetication with id {AutheticationId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                _logger.LogInformation("Authetication with id {AutheticationId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with id {AutheticationId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var existingAuthetication = await _service.GetByIdAsync(id);
                if (existingAuthetication is null)
                {
                    _logger.LogWarning("Authetication with id {AutheticationId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingAuthetication.Id);

                _logger.LogInformation("Authetication with id {AutheticationId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with id {AutheticationId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            return await SendMessageAsync("getall", new AutheticationDto(), "POST - MessageGetAll", "consumption-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            return await SendMessageAsync("getbyid", new AutheticationDto { Id = id }, "POST - MessageGetById", "consumption-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] AutheticationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await SendMessageAsync("create", dto, "POST - MessageCreate", "consumption-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] AutheticationDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            return await SendMessageAsync("update", dto, "PUT - MessageUpdate", "consumption-queue");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            return await GetResultAsyncInternal(id, (action, messageResponse) =>
            {
                return action.ToLowerInvariant() switch
                {
                    "getall" => JsonSerializer.Deserialize<IEnumerable<AutheticationDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<AutheticationDto>(messageResponse),
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

            return await SendMessageAsync("delete", new AutheticationDto { Id = id }, "DELETE - MessageDelete", "consumption-queue");
        }

        #endregion

        #region Additional Methods  

        #endregion
    }
}