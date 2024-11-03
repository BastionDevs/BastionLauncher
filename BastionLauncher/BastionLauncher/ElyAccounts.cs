using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BastionLauncher
{
    class ElyAccounts
    {

        public static string PwdAuth(string username, string password, string clienttoken, bool requestuser)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://authserver.ely.by/auth/authenticate");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write("{\r\n  \"username\": \"" + username + "\",\r\n  \"password\": \"" + password + "\",\r\n  \"clientToken\": \"" + clienttoken + "\",\r\n  \"requestUser\": \"" + requestuser + "\"\r\n}");
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        // Return or log error details along with status code
                        Console.Write($"Error: {errorResponse.StatusCode}, Body: {error}");
                        return error;
                    }
                }
            }
        }

        public static string Refresh(string accesstoken)
        {
            string resp = NetUtils.POSTReqJSON($"https://authserver.ely.by/auth/refresh", "{\r\n  \r\n  \"accessToken\": \"" + accesstoken + "\"\r\n}");
            if (!resp.Contains("{\r\n    \"error\": \"")) { return resp; } else { return "ERROR:" + resp; }
        }

        public static bool ValidateToken(string accesstoken)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://authserver.ely.by/auth/validate");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write("{\r\n \"accessToken\": \"" + accesstoken + "\"\r\n}");
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    if (string.IsNullOrEmpty(streamReader.ReadToEnd()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            // Return or log error details along with status code
                            Console.Write($"Error: {errorResponse.StatusCode}, Body: {error}");
                            return false;
                        }
                    }
                } else { return false; }

            }
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
