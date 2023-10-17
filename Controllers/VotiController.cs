using HtmlAgilityPack;
using mastercom_api.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace mastercom_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/x-www-form-urlencoded")]
    public class VotiController : ControllerBase
    {

        private MastercomAPI mastercomAPI;
        public VotiController(IOptions<Config> options)
        {
            mastercomAPI = new MastercomAPI(options);
        }

        public string CleanupString(string input)
        {
            return input.Replace("  ", string.Empty).Replace("\n", string.Empty);
        }

        public string[] GetDateAndType(string input)
        {
            if (input.Contains("Orale"))
                return input.Split("Orale").Append("Orale").ToArray();
            else if (input.Contains("Scritto"))
                return input.Split("Scritto").Append("Scritto").ToArray();
            else
                return input.Split("Pratico").Append("Pratico").ToArray();
        }
        public Voto ParseVoto(string index0, string index1, string index2)
        {
            var dataTipo = GetDateAndType(CleanupString(index1));
            var materiaInsegnante = index2.Replace("  ", string.Empty).Split('\n');
            return new Voto
            {
                VotoNum = Convert.ToInt16(CleanupString(index0)),
                Materia = materiaInsegnante[1],
                Insegnante = materiaInsegnante[3],
                Data = dataTipo[0],
                Tipo = dataTipo[1]
            };
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromForm] GenericEndpointRequest payload)
        {
            var htmlDoc = await mastercomAPI.doRequest(payload, "voti");

            var votiRawTable = new HtmlDocument();
            votiRawTable.LoadHtml(htmlDoc.DocumentNode.SelectNodes("//div/table").First().InnerHtml);
            var votiTable = votiRawTable.DocumentNode.SelectNodes("//tr").Select(x =>
            {
                if (x.InnerText.Contains("Voto"))
                    return null;
                var document = new HtmlDocument();
                document.LoadHtml(x.InnerHtml);
                return document.DocumentNode.SelectNodes("//td").Select(y => y.InnerText).ToArray();
            }).ToList().Skip(1).ToList();
            var voti = new List<Voto>();
            foreach (var votoRaw in votiTable)
            {
                if (votoRaw != null)
                    voti.Add(ParseVoto(votoRaw[0], votoRaw[1], votoRaw[2]));
            }
            return Ok(voti);
        }
    }
}