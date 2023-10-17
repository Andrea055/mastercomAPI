using HtmlAgilityPack;
using mastercom_api.Helper;
using mastercom_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace mastercom_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/x-www-form-urlencoded")]
    public class LoginController : ControllerBase
    {
        private MastercomAPI mastercomAPI;
        public LoginController(IOptions<Config> options) {
            mastercomAPI = new MastercomAPI(options);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] LoginRequest loginModel)
        {
            var htmlDoc = await mastercomAPI.doLogin(loginModel);

            var title = htmlDoc.DocumentNode.SelectSingleNode("//title");

            if (title.InnerHtml.Contains("Login"))
            {
                return Unauthorized();
            }
            else
            {
                return Ok(new LoginResponse()
                {
                    currentKey = htmlDoc.GetElementbyId("current_key").GetAttributeValue("value", ""),
                    currentUser = Convert.ToInt32(htmlDoc.GetElementbyId("current_user").GetAttributeValue("value", "")),
                    dbKey = htmlDoc.GetElementbyId("db_key").GetAttributeValue("value", ""),
                    tipoUtente = htmlDoc.GetElementbyId("tipo_utente").GetAttributeValue("value", ""),
                });
            }
        }
    }
}