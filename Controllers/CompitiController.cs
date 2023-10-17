using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using mastercom_api.Helper;
using Microsoft.Extensions.Options;

namespace mastercom_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompitiController : ControllerBase
    {
        private MastercomAPI mastercomAPI;
        public CompitiController(IOptions<Config> options)
        {
            mastercomAPI = new MastercomAPI(options);
        }

        private bool isDay(string input)
        {
            return input.Contains("Lunedì") ||
                input.Contains("Martedì") ||
                input.Contains("Mercoledì") ||
                input.Contains("Giovedì") ||
                input.Contains("Venerdì") ||
                input.Contains("Sabato") ||
                input.Contains("Domenica");
        }

        private string CleanupString(string input)
        {
            return input.Replace("  ", string.Empty);
        }

        private string getWhenInserito(string input)
        {
            try
            {
                return input.Split("Inserito")[1];
            }
            catch (Exception)
            {
                return "";
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromForm] GenericEndpointRequest payload)
        {
            var htmlDoc = await mastercomAPI.doRequest(payload, "argomenti-compiti");

            var raw = htmlDoc.DocumentNode.SelectNodes("//table").First().InnerHtml;
            var compitiRawTable = new HtmlDocument();
            compitiRawTable.LoadHtml(raw);
            var tableElements = compitiRawTable.DocumentNode.SelectNodes("//tr/td").ToArray();

            var events = new List<Compito>();
            var currentDay = new List<string>();
            for (var i = 0; i < tableElements.Length; i++)  // Use a classic for loop because we need index and index+1 for the information
            {
                var eventRaw = CleanupString(tableElements[i].InnerText);
                if (isDay(eventRaw))
                {
                    currentDay = eventRaw.Replace("\n\n", string.Empty).Split("\n").ToList();   // Number\nMonth\nWeekday
                }
                else
                {
                    var detailRaw = eventRaw.Replace("\n\n", string.Empty).Split("\n").ToList();
                    detailRaw.RemoveAt(0);
                    detailRaw.RemoveAt(0);
                    events.Add(new Compito()
                    {
                        date = string.Format("{0} {1}", currentDay[1], currentDay[2]),
                        materia = detailRaw[0],
                        details = string.Join(" ", detailRaw.Skip(1)).Replace("Da fare", string.Empty).Split("Inserito")[0].Trim(),  // Sometime there are more details
                        inserito = getWhenInserito(string.Join(" ", detailRaw.Skip(1))).Trim()
                    });
                }
            }

            return Ok(events);
        }
    }
}