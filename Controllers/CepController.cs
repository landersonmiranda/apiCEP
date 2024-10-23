using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoCep.Models;

namespace ProjetoCep.Controllers
{
    public class CepController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ConsultaCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
            {
                return View("Index");
            }

            Endereco endereco = await ObterEnderecoAsync(cep);

            if (endereco == null)
            {
                ViewBag.Error = "CEP não encontrado.";
                return View("Index");
            }

            return View("Resultado", endereco);
        }

        private async Task<Endereco> ObterEnderecoAsync(string cep)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://viacep.com.br/ws/{cep}/json/";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Endereco endereco = JsonConvert.DeserializeObject<Endereco>(jsonResponse);
                    return endereco;
                }
            }

            return null;
        }
    }
}
