using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace memo_backend_net
{
    public static class GetScreenText
    {
        [FunctionName("GetScreenText")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            var connectionString = ConnectionString.DBConnectionString;
            var resultDict = new Dictionary<string, string>();

            var s = "";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var text = "SELECT * FROM test";
                using (SqlCommand cmd = new SqlCommand(text, conn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            s += String.Format("{0}, {1}", reader[0], reader[1]);
                            log.LogInformation($"{s}");
                        }
                    }
                }
            }

            resultDict["sqlResult"] = s;

            var resultJson = JsonConvert.SerializeObject(resultDict);

            return new OkObjectResult(resultJson);
        }
    }
}
