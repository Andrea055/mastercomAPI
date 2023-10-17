using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using mastercom_api.Helper;
using Microsoft.Extensions.Options;

namespace mastercom_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private MastercomAPI mastercomAPI;
        public AgendaController(IOptions<Config> options)
        {
            mastercomAPI = new MastercomAPI(options);
        }

        private string CleanupString(string input)
        {
            return input.Replace("  ", string.Empty);
        }

        private string GetInsegnante(string input)
        {
            if (!input.Contains("("))
            {
                return "";
            }
            else
            {
                return input.Split("-(")[1].Replace(")-", string.Empty);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromForm] GenericEndpointRequest payload)
        {
            var htmlDoc = await mastercomAPI.doRequest(payload, "agenda");

            var agendaRawTable = new HtmlDocument();
            var raw = htmlDoc.DocumentNode.SelectNodes("//table").First().InnerHtml;
            agendaRawTable.LoadHtml(raw);
            var tableElements = agendaRawTable.DocumentNode.SelectNodes("//tr/td").ToArray();

            var events = new List<AgendaEvent>();
            for (var i = 0; i < tableElements.Length; i++)  // Use a classic for loop because we need index and index+1 for the information
            {
                var dateRaw = CleanupString(tableElements[i].InnerText).Replace("\n\n", "-");
                var detailsRaw = CleanupString(tableElements[++i].InnerText).Replace("\n\n", "-").Replace("&nbsp;", string.Empty);

                var date = dateRaw.Split('-');
                var details = detailsRaw.Split("--");
                var dateParsed = string.Format("{0} {1}", date[1], date[0]).Replace("\n", " "); // Needed for substring
                events.Add(new AgendaEvent()
                {
                    date = dateParsed.Substring(1, dateParsed.Length - 2),
                    orario = new string(details[0].Replace("\n", string.Empty).Skip(1).ToArray()),
                    eventName = details[1].Replace("\n", string.Empty).Replace("-", string.Empty).Split('(')[0],
                    insegnante = GetInsegnante(details[1]),
                    isToday = dateRaw.Contains("OGGI")
                });
            }
            return Ok(events);
        }
    }
}