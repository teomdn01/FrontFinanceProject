using System.Collections;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Brokers.Freedom.Configuration;

public class PublicApiClient
{
    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public int Version { get; set; } = 1;
    public HttpClient HttpClient { get; set; }

    public PublicApiClient(string apiKey = "e8a25afdaf2fafbcf099744f5f24640e", string apiSecret = "85b4e174f7efce916b9fa1099022f2cf6c3412ad", int version = 1)
    {
        ApiUrl = "https://tradernet.ru/api";
        ApiKey = apiKey;    
        ApiSecret = apiSecret;
        Version = version;
        HttpClient = new HttpClient();  
    }

    public string PreSign(IDictionary<string, object> dict)
    {
        string s = "";
        foreach (var kvp in dict)
        {
            if (IsDictionary(kvp.Value))
            {
                s += kvp.Key + "=" + PreSign((IDictionary<string, object>)kvp.Value) + "&";
            }
            else
            {
                s += kvp.Key + "=" + kvp.Value + "&";
            }
        }

        return s.Remove(s.Length - 1);
    }

    public string HttpEncode(IDictionary<string, object> d)
    {
        string s = "";
        foreach (string i in d.Keys)
        {
            if (d[i] is Dictionary<string, object>)
            {
                var nestedDict = (Dictionary<string, object>)d[i];
                foreach (string into in nestedDict.Keys)
                {
                    if (nestedDict[into] is Dictionary<string, object>)
                    {
                        var subNestedDict = (Dictionary<string, object>)nestedDict[into];
                        foreach (string subInto in subNestedDict.Keys.OrderBy(k => k))
                        {
                            if (subNestedDict[subInto] is Dictionary<string, object>)
                            {
                                s += HttpEncode((Dictionary<string, object>)subNestedDict[subInto]);
                            }
                            else
                            {
                                s += i + "[" + into + "][" + subInto + "]=" + subNestedDict[subInto].ToString() + "&";
                            }
                        }
                    }
                    else
                    {
                        s += i + "[" + into + "]=" + nestedDict[into].ToString() + "&";
                    }
                }
            }
            else
            {
                s += i + "=" + d[i].ToString() + "&";
            }
        }

        return s.Substring(0, s.Length - 1);
    }

    public async Task<HttpResponseMessage> SendRequest(string method, object aParams = null, string format = "JSON")
    {
        var aReq = new SortedDictionary<string, object>
        {
            { "cmd", method }
        };
        if (aParams != null)
        {
            aReq["params"] = aParams;
        }

        if (Version != 1 && !string.IsNullOrEmpty(ApiKey))
        {
            aReq["apiKey"] = ApiKey;
        }

        //aReq["nonce"] = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        aReq["nonce"] = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() * 100000;

        var preSig = PreSign(aReq);
        var preSigEncoded = HttpEncode(aReq);

        // Ignore errors for local ssl connection

        HttpResponseMessage res = null;

        // Create signature and send request based on V1 or V2
        if (Version == 1)
        {
            aReq["sig"] = ComputeHMACSHA256(ApiSecret, string.Empty);
            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "q", JsonSerializer.Serialize(aReq) } });
            var response = HttpClient.PostAsync(ApiUrl, content).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
        else
        {
            var signature = ComputeHMACSHA256(ApiSecret, preSig);
            var content = new FormUrlEncodedContent(ToDictionary(preSigEncoded));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //ApiUrl += "/v2/cmd/" + method;
            //HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-NtApi-Sig", signature);

            var msg = new HttpRequestMessage(HttpMethod.Post, ApiUrl + "/v2/cmd/" + method);
            msg.Content = content;
            msg.Headers.TryAddWithoutValidation("X-NtApi-Sig", signature);
            var response = HttpClient.SendAsync(msg).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
    
    private static Dictionary<string, string> ToDictionary(string queryString)
    {
        var dictionary = new Dictionary<string, string>();
        var query = HttpUtility.ParseQueryString(queryString);
        foreach (var key in query.AllKeys)
        {
            dictionary[key] = query[key];
        }
        return dictionary;
    }
    
    public static string ComputeHMACSHA256(string key, string data)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            byte[] hashBytes = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }


    private bool IsDictionary(object o)
    {
        if (o == null) return false;
        return o is IDictionary &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
    }
}