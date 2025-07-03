using FSI.AccessAuthentication.Api.Controllers.Base;
using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.AccessAuthentication.Api.Controllers
{
    [ApiController]
    [Route("api/authentications/async")]
    public class AuthenticationControllerAsync : BaseAsyncController<AuthenticationDto>
    {
        private readonly IAuthenticationAppService _service;

        public AuthenticationControllerAsync(IAuthenticationAppService service, ILogger<AuthenticationControllerAsync> logger,
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
                    _logger.LogWarning("Authentication with id {AuthenticationId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving traffic with id {AuthenticationId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthenticationRequestDto dto)
        {
            try
            {
                // Validação por atributos (se houver)
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Validação de regra de negócio + banco
                var validation = await _service.ValidationResultAccessAsync(dto);

                if (!validation.IsAuthentication)
                {
                    ModelState.AddModelError(string.Empty, validation.ErrorMessage);
                    return BadRequest(ModelState);
                }

                // Prossegue com a inserção
                var authenticationResponse = await _service.InsertAsync(validation);

                validation.Id = authenticationResponse;

                _logger.LogInformation("Authentication created with id {AuthenticationId}", validation.Id);
                return CreatedAtAction(nameof(GetById), new { id = validation.Id }, validation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {@AuthenticationDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] AuthenticationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for user update: {@AuthenticationDto}", dto);
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
                    _logger.LogWarning("Authetication with id {AuthenticationId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                _logger.LogInformation("Authetication with id {AutheticationId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with id {AuthenticationId}", id);
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
                    _logger.LogWarning("Authetication with id {AuthenticationId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingAuthetication.Id);

                _logger.LogInformation("Authetication with id {AuthenticationId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with id {AuthenticationId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            return await SendMessageAsync("getall", new AuthenticationDto(), "POST - MessageGetAll", "authentication-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            return await SendMessageAsync("getbyid", new AuthenticationDto { Id = id }, "POST - MessageGetById", "authentication-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] AuthenticationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await SendMessageAsync("create", dto, "POST - MessageCreate", "authentication-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] AuthenticationDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            return await SendMessageAsync("update", dto, "PUT - MessageUpdate", "authentication-queue");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            return await GetResultAsyncInternal(id, (action, messageResponse) =>
            {
                return action.ToLowerInvariant() switch
                {
                    "getall" => JsonSerializer.Deserialize<IEnumerable<AuthenticationDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<AuthenticationDto>(messageResponse),
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

            return await SendMessageAsync("delete", new AuthenticationDto { Id = id }, "DELETE - MessageDelete", "authentication-queue");
        }

        #endregion
    }
}