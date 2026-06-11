using ClientManagement.Caching;
using ClientManagement.Models;
using ClientManagement.Repository;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICacheProvider _cacheProvider;

        public ClientController(IClientRepository clientRepository, ICacheProvider cacheProvider)
        {
            _clientRepository = clientRepository;
            _cacheProvider = cacheProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] string? term, [FromQuery] string? sort, 
            [FromQuery] int page = 1,
            [FromQuery] int limit = 5)
        {
            string cacheKey = $"Clients_{term}_{sort}_{page}_{limit}";

            if (!_cacheProvider.TryGetValue(cacheKey, out PagedClientResult? result))
            {
                result = await _clientRepository
                    .GetAllClients(term, sort, page, limit);

                if (result == null)
                    return NotFound("No clients found.");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(30),

                    SlidingExpiration =
                        TimeSpan.FromSeconds(30),

                    Size = 1000
                };

                _cacheProvider.Set(
                    cacheKey,
                    result,
                    cacheEntryOptions);
            }

            Response.Headers.Append(
                "X-Total-Count",
                result!.TotalCount.ToString());

            Response.Headers.Append(
                "X-Total-Pages",
                result.TotalPages.ToString());

            return Ok(result.Clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            if (!_cacheProvider.TryGetValue(CacheKeys.Client, out Client client))
            {
                client = await _clientRepository.GetClientById(id);

                if (client == null)
                    return NotFound("Client not found.");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1000
                };
                _cacheProvider.Set(CacheKeys.Client, client, cacheEntryOptions);
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
