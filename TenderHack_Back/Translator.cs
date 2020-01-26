using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

namespace TenderHack_Back
{
    public class TranslationResult
    {
        public DetectedLanguage DetectedLanguage { get; set; }
        public TextResult SourceText { get; set; }
        public Translation[] Translations { get; set; }
    }
    public class DetectedLanguage
    {
        public string Language { get; set; }
        public float Score { get; set; }
    }
    public class TextResult
    {
        public string Text { get; set; }
        public string Script { get; set; }
    }
    public class Translation
    {
        public string Text { get; set; }
        public TextResult Transliteration { get; set; }
        public string To { get; set; }
        public Alignment Alignment { get; set; }
        public SentenceLength SentLen { get; set; }
    }
    public class Alignment
    {
        public string Proj { get; set; }
    }
    public class SentenceLength
    {
        public int[] SrcSentLen { get; set; }
        public int[] TransSentLen { get; set; }
    }
    class Translator
    {
        private const string Endpoint =
            "https://api-eur.cognitive.microsofttranslator.com";

        private const string Key = "af2d25e6d9d540239a426f4c26744761";

        public async Task<List<string>> TranslateTextRequest(string route, string inputText)
        {
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                /*{
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(Endpoint + route),
                    Content = new StringContent(requestBody, Encoding.UTF8,
                        @"application/json")
                };*/
                request.Method = HttpMethod.Post;
               
                request.RequestUri = new Uri(Endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", Key);
                //request.Headers.Add("Ocp-Apim-Subscription-Region", "northeurope");
                //request.Headers.Add("Ocp-Apim-Subscription-Key", Key);

                var response = await client.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var deserializedOutput = 
                        JsonConvert.DeserializeObject<TranslationResult[]>(result);
                    var res = new List<string>();
                    foreach (TranslationResult o in deserializedOutput)
                    {
                        foreach (Translation t in o.Translations)
                        {
                            res.Add(t.Text);
                        }
                    }

                    return res;
                }
                return new List<string>();
            }
        }

    }
}
