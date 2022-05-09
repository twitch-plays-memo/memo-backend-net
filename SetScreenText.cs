using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class SetScreenText
    {
        [FunctionName("SetScreenText")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string setText = req.Query["setText"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            setText = setText ?? data?.setText;

            var resultDict = new Dictionary<string, string>();
            resultDict["message"] = "hei";
            resultDict["text"] = setText;

            var result = JsonConvert.SerializeObject(resultDict);

            return new OkObjectResult(result);
        }
    }
}
