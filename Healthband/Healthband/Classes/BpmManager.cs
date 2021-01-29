using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Org.Json;

namespace Healthband.Classes
{
    class BpmManager
    {    

        public string test()
        {
            string URL_List = "http://p.aeffl.pt/17791/php/ListBPM.php?id_user=";
            return URL_List + App._id;
        }


        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Connection", "close");
            return client;
        }

        public async Task<IEnumerable<BPM>> GetBPM()
        {
            HttpClient client = GetClient();
            BpmManager manager = new BpmManager();
            string URL_List = manager.test().ToString();

            HttpResponseMessage response = await client.GetAsync(URL_List);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<BPM>>(responseBody);
        }


        public async Task<IEnumerable<UserBpmResponse>> SendBPM(int _id_user)
        {
            HttpClient client = GetClient();
            BpmManager manager = new BpmManager();
            string URL_List = manager.test().ToString();

            string url = URL_List;
            string contentType = "application/json";

            JObject json = new JObject
            {
                {"ID_user", _id_user}
            };

            var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
            var data = await response.Content.ReadAsStringAsync();
            Dictionary<string, string> text = new Dictionary<string, string>();
            List<UserBpmResponse> lista_bpm = new List<UserBpmResponse>();

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            try
            {
                lista_bpm = JsonConvert.DeserializeObject<List<UserBpmResponse>>(responseBody);
                Console.WriteLine(lista_bpm.Count);
            }
            catch (Exception ex) {
               Console.WriteLine("Excepetion" + ex.Message);
            }
            return lista_bpm;
        }

        public async void addBPM(int value_, DateTime date_, int id_)
        {
            string URL_add = "https://p.aeffl.pt/17791/php/addBPM.php";
            string url = URL_add;
            string contentType = "application/json";


            JObject json = new JObject
            {
                { "ValueBPM", value_},
                { "DateBPM", date_},
                { "IdUser", id_}
            };


            HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));

            var data = await response.Content.ReadAsStringAsync();

        }

    }
}