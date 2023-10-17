using HtmlAgilityPack;
using mastercom_api.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace mastercom_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/x-www-form-urlencoded")]
    public class OrarioController : ControllerBase
    {

        private MastercomAPI mastercomAPI;
        public OrarioController(IOptions<Config> options)
        {
            mastercomAPI = new MastercomAPI(options);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromForm] GenericEndpointRequest payload)
        {
            var htmlDoc = await mastercomAPI.doRequest(payload, "orario");

            var jsonOrarioRaw = htmlDoc
                .DocumentNode
                .SelectNodes("//script")[2]
                .InnerHtml
                .Replace("var riepilogo_orario = JSON.parse('", string.Empty)
                .Replace("');", string.Empty);

            var orarioParsed = JsonSerializer.Deserialize<OreTotale>(jsonOrarioRaw);
            return Ok(orarioParsed);
        }
    }
}