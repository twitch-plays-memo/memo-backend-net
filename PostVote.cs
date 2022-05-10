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
    public static class PostVote
    {
        [FunctionName("PostVote")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string id = req.Query["id"];
            string card_id = req.Query["card_id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            card_id = card_id ?? data?.card_id;
            id = id ?? data?.id;

            var resultDict = new Dictionary<string, string>();

            var connectionString = ConnectionString.DBConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var text = $"INSERT INTO VOTES ({id},{card_id})";
                           //$"SELECT * FROM (SELECT '{id}' AS id) AS temp" +
                           //$"WHERE NOT EXISTS (" + 
                           //$"SELECT id FROM VOTES WHERE id='{id}'" + 
                           //$") LIMIT 1;";

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
