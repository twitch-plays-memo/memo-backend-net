using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memo_backend_net
{
    public static class ConnectionString
    {
        public static string DBConnectionString = 
            "Server=tcp:twitch-plays.database.windows.net,1433;Initial Catalog=memo;Persist Security Info=False;User ID=memo;Password=Plays_987123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
