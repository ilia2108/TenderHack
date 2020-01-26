using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

namespace TenderHack_Back
{
    public class Reviews
    {
        private string Endpoint = "https://myaibazara.azurewebsites.net/api/v1/addReview";

        public async Task<ReviewResponse> MakeRequest(string text)
        {
            using (var client = new HttpClient())
            {
                object obj = new
                {
                    uid = "9ba81b2a-0d84-4c62-b11f-62fca35f5b21",
                    prodId = "f486384f-2b9a-4494-81d4-ea93bc1f6bc9",
                    text
                };

                var payload = JsonConvert.SerializeObject(obj);
                var content = new StringContent(payload, Encoding.UTF8, @"application/json");

                var response = await client.PostAsync(Endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var review = JsonConvert.DeserializeObject<ReviewResponse>(body);
                        return review;
                    }
                    catch
                    {
                        return new ReviewResponse();
                    }
                }
                return new ReviewResponse();

            }

        }
    }
}
