using HtmlAgilityPack;
using mastercom_api.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace mastercom_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/x-www-form-urlencoded")]
    public class AssenzeController : ControllerBase
    {

        private MastercomAPI mastercomAPI;
        public AssenzeController(IOptions<Config> options)
        {
            mastercomAPI = new MastercomAPI(options);
        }

        public string CleanupString(string input)
        {
            return input.Replace("  ", string.Empty);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromForm] GenericEndpointRequest payload)
        {
            var htmlDoc = await mastercomAPI.doRequest(payload, "assenze");

            var events = new List<Assenza>();

            var tableElements = htmlDoc.DocumentNode.SelectNodes("//tr/td").ToArray();
            for (var i = 0; i < tableElements.Length; i++)  // Use a classic for loop because we need index and index+1 for the information
            {
                var dateRaw = CleanupString(tableElements[i].InnerText).Replace("\n\n", "-").Replace("Giustificata", " Giustificata");
                if (dateRaw.Length == 3)
                {
                    continue;
                }
                var detailsRaw = CleanupString(tableElements[++i].InnerText).Replace("\n", "-").Replace("Giustificata", " Giustificata");

                var date = dateRaw.Split('-');
                var details = detailsRaw.Split('-');
                events.Add(new Assenza()
                {
                    Date = string.Format("{0} {1}", date[1], date[0]).Replace("\n", string.Empty),
                    Motivo = details[1],
                    Ora = details[2],    // This string is hour detail
                    Stato = details[4]  // Index 3 is empty
                });
            }
            return Ok(events);
        }
    }
}