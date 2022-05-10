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
    public static class GetGameStateAndStats
    {
        [FunctionName("GetGameStateAndStats")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var connectionString = ConnectionString.DBConnectionString;
            var resultDict = new Dictionary<string, string>();

            using (var conn = new SqlConnection(connectionString))
            {
                // Get game state
                // TODO: SELECT to get game state
                var commandState = "SELECT * FROM GAME_STATE";
                using (SqlCommand cmd = new SqlCommand(commandState, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            log.LogInformation($"{reader[0]}");
                            resultDict["state"] = reader["state"].ToString();
                        }
                    }
                }

                // Get game stats
                // TODO: SELECT score, time, turns from the correct database table
                var command = "SELECT * FROM GAME_STATS";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            log.LogInformation($"id={reader[0]}, score={reader[1]}, time={reader[2]}, turns={reader[3]}, total_cards={reader[4]}");
                            resultDict["score"] = reader["score"].ToString();
                            resultDict["time"] = reader["time"].ToString();
                            resultDict["turns"] = reader["turns"].ToString();
                            resultDict["total_cards"] = reader["total_cards"].ToString();
                        }
                    }
                }
            }

            var resultJson = JsonConvert.SerializeObject(resultDict);
            return new OkObjectResult(resultJson);
        }
    }
}
