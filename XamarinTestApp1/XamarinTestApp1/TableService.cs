using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XamarinTestApp1
{
    class TableService
    {
        const string Url = "http://www.nonamesite1.somee.com/api/Tables/";
        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем всех друзей
        public async Task<IEnumerable<Table>> Get()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Table>>(result);
        }

        // добавляем одного друга
        public async Task<Table> Add(Table table)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(table),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Table>(
                await response.Content.ReadAsStringAsync());
        }
        // обновляем друга
        public async Task<Table> Update(Table table)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url + "/" + table.Id,
                new StringContent(
                    JsonConvert.SerializeObject(table),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Table>(
                await response.Content.ReadAsStringAsync());
        }
        // удаляем друга
        public async Task<Table> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Table>(
               await response.Content.ReadAsStringAsync());
        }
    }
}

