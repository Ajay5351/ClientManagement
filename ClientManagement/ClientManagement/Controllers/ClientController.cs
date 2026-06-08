using ClientManagement.Data;
using ClientManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientRepository.GetAllClients();

            if (clients == null || !clients.Any())
            {
                return NotFound("No clients found.");
            }

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _clientRepository.GetClientById(id);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            return Ok(client);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            try
            {
                var addedClient = await _clientRepository.AddClient(client);
                return CreatedAtAction(nameof(GetClientById), new { id = addedClient.ClientId }, addedClient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
        {
            if (id != client.ClientId)
            {
                return BadRequest("Client ID mismatch.");
            }

            try
            {
                var updatedClient = await _clientRepository.UpdateClient(client);
                return Ok(updatedClient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var result = await _clientRepository.DeleteClient(id);

                if (!result)
                {
                    return NotFound("Client not found.");
                }

                return Ok("Client deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
