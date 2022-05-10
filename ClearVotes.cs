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
    public static class ClearVotes
    {
        [FunctionName("ClearVotes")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var resultDict = new Dictionary<string, string>();

            var connectionString = ConnectionString.DBConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var text = $"TRUNCATE TABLE VOTES;";
                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                    resultDict["sql_result"] = rows.ToString();
                }
            }

            var resultJson = JsonConvert.SerializeObject(resultDict);
            return new OkObjectResult(resultJson);
        }
    }
}
