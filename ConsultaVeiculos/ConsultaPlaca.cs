using ConsultaVeiculos.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaVeiculos
{
    public class ConsultaPlaca
    {
        public async Task<ModelConsulta> BuscaPlaca(string placa, string uri)
        {
            ModelConsulta dados = null;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("chave", "chavedemo");
                var req = new HttpRequestMessage(HttpMethod.Post, uri);
                var reqBody = JsonConvert.SerializeObject(new { placa = placa });

                req.Content = new StringContent(reqBody, Encoding.UTF8, "application/json");
                var resp = client.SendAsync(req).Result;

                if (resp.IsSuccessStatusCode)
                {
                    var VeiculoJsonString = await resp.Content.ReadAsStringAsync();
                    dados = JsonConvert.DeserializeObject<ModelConsulta>(VeiculoJsonString);

                }
                else
                {

                    throw new Exception($"Houve um problema ao buscar a placa informada! \n Error - {resp.ReasonPhrase}");
                }
            }
            catch (Exception ex) { }
            
            return dados;
        }
    }
}
