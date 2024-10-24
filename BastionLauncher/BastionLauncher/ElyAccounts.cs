using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastionLauncher
{
    class ElyAccounts
    {

        public static string PwdAuth(string username, string password, string clienttoken, bool requestuser)
        {
            string resp = NetUtils.POSTReqJSON($"https://authserver.ely.by/auth/authenticate", "{\r\n  \"username\": \""+username+"\",\r\n  \"password\": \""+password+"\",\r\n  \"clientToken\": \""+clienttoken+"\",\r\n  \"requestUser\": \""+requestuser+"\"\r\n}");
            if (!resp.Contains("{\r\n    \"error\": \"")) { return resp; } else { return "ERROR:"+resp; }
        }

        public static string Refresh(string accesstoken)
        {
            string resp = NetUtils.POSTReqJSON($"https://authserver.ely.by/auth/refresh", "{\r\n  \r\n  \"accessToken\": \"" + accesstoken + "\"\r\n}");
            if (!resp.Contains("{\r\n    \"error\": \"")) { return resp; } else { return "ERROR:" + resp; }
        }

        public static bool ValidateToken(string username, string password, string clienttoken, bool requestuser)
        {
            string resp = NetUtils.POSTReqJSON($"https://authserver.ely.by/auth/validate", "{\r\n  \"username\": \"" + username + "\",\r\n  \"password\": \"" + password + "\",\r\n  \"clientToken\": \"" + clienttoken + "\",\r\n  \"requestUser\": \"" + requestuser + "\"\r\n}");
            if (!resp.Contains("{\r\n    \"error\": \"")) { return true; } else { return false; }
        }

        public static bool Signout(string username, string password)
        {
            string resp = NetUtils.POSTReqJSON($"https://authserver.ely.by/auth/signout", "{\r\n  \"username\": \"" + username + "\",\r\n  \"password\": \"" + password + "\"\r\n}");
            if (!resp.Contains("{\r\n    \"error\": \"")) { return true; } else { return false; }
        }

        public static bool InvalidateToken(string accessToken, string clientToken)
        {
            string resp = NetUtils.POSTReqJSON($"https://authserver.ely.by/auth/invalidate", "{\r\n  \"accessToken\": \"" + accessToken + "\",\r\n  \"clientToken\": \"" + clientToken + "\"\r\n}");
            if (!resp.Contains("{\r\n    \"error\": \"")) { return true; } else { return false; }
        }

    }
}
