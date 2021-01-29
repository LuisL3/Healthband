using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Healthband.Classes
{
    class UserManager
    {
        const string URL = "https://p.aeffl.pt/17791/php/ListUserData.php";
        const string URL_add = "https://p.aeffl.pt/17791/php/adduser.php";
        const string URL_login = "https://p.aeffl.pt/17791/php/login.php";

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Connection", "close");

            return client;
        }

        public async Task<IEnumerable<User>> GetUser()
        {
            HttpClient client = GetClient();

            HttpResponseMessage response = await client.GetAsync(URL);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<User>>(responseBody);
        }

        public async Task<IEnumerable<UserLoginResponse>> Login_user(string email, string password)
        {
            HttpClient client = GetClient();

            string url = URL_login;
            string contentType = "application/json";

            JObject json = new JObject
            {
                {"User_Email", email },
                {"User_Password", password }
            };

            var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
            var data = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

           Dictionary<string, string> text = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

            string resposta = text["response"];
            int ID_user = int.Parse(text["ID_user"]);

            UserLoginResponse _respon = new UserLoginResponse(resposta, ID_user);

            IEnumerable<UserLoginResponse> resp = new List<UserLoginResponse>() { _respon };
            return resp;
        }
        public async void addUser(string name_, string pass_, string email_)
        {

            string url = URL_add;
            string contentType = "application/json";
            
            //Envia o Json para a base de dados
            JObject json = new JObject
            {
                { "User_Name", name_},
                { "User_Password", pass_},
                { "User_Email", email_}

            };

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));

            var data = await response.Content.ReadAsStringAsync();
        } 
    }
}