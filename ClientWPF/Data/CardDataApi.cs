using ClientWPF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientWPF.Data
{
    class CardDataApi
    {
        public static event Action<string, string> Events;


        private HttpClient httpClient { get; set; }


        public CardDataApi()
        {
            httpClient = new HttpClient();
        }
        

        /// <summary>
        /// GET
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Card> GetCards()
        {
            string url = @"http://localhost:50519/api/cards";

            string json = httpClient.GetStringAsync(url).Result;
            return JsonConvert.DeserializeObject<IEnumerable<Card>>(json);
        }


        /// <summary>
        /// POST
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            string url = @"http://localhost:50519/api/cards";

            try
            {
                var r = httpClient.PostAsync(
                    requestUri: url,
                    content: new StringContent(JsonConvert.SerializeObject(card), Encoding.UTF8,
                    mediaType: "application/json")
                    ).Result;

                Events?.Invoke("Ok", "#FF0ED33A");
            }
            catch 
            {
                Events?.Invoke("Ошибка отправки POST" , "#FFDC0808");
            }

        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCard(int id)
        {
            string url = @"http://localhost:50519/api/cards/" + $"{id}";

            try
            {
                var r = httpClient.DeleteAsync(requestUri: url);

                Events?.Invoke("Ok", "#FF0ED33A");
            }
            catch
            {
                Events?.Invoke("Ошибка отправки DELETE", "#FFDC0808");
            }
        }


        /// <summary>
        /// Put
        /// </summary>
        /// <param name="id"></param>
        /// <param name="card"></param>
        public void PutCard(int id, Card card)
        {
            string url = @"http://localhost:50519/api/cards/" + $"{id}";


            try
            {
                var r = httpClient.PutAsync(
                    requestUri: url,
                    content: new StringContent(JsonConvert.SerializeObject(card), Encoding.UTF8,
                    mediaType: "application/json")
                    ).Result;

                Events?.Invoke("Ok", "#FF0ED33A");
            }
            catch 
            {
                Events?.Invoke("Ошибка отправки PUT", "#FFDC0808");

            }
        }

    }
}
