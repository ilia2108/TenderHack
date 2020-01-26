using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace TenderHack_Back
{
    public static class Function1
    {
        [FunctionName("AddToDB")]
        public static async Task<IActionResult> AddToDb(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Proceeding request to add to DB");

            string key = req.Query["key"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            key = key ?? data?.key;

            log.LogInformation("key is been read from param");
            DBConnector db = new DBConnector();
            log.LogInformation("DB created");
            db.Add(key);
            log.LogInformation("Key is added");
            return new OkObjectResult("Added");
        }

        [FunctionName("SelectKey")]
        public static IActionResult SelectKey([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SelectKey functions triggered");
            DBConnector connector = new DBConnector();
            log.LogInformation("DB connector created");
            var key = connector.GetAll();
            log.LogInformation("Key was given back");
            return new OkObjectResult(key);
        }

        [FunctionName("AddReview")]
        public async static Task<IActionResult> AddReview([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Proceeding request to add to DB");

            string translate = req.Query["text"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            translate = translate ?? data?.key;


            var route = "/translate?api-version=3.0&to=en";
            var translations = await new Translator().TranslateTextRequest(route, translate);
            string res = string.Empty;
            foreach (var str in translations)
            {
                res += str + "\n";

            }
            res = res.Remove(res.Length - 1);
            return new OkObjectResult(res);
        }
    }
}
