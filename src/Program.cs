using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program
{
    static readonly HttpClient client = new HttpClient();
    static async Task Main(string[] args)
    {
        var refToken =  "eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjU4OTk1MzNiLTZhYjAtNDNlYi1hNjQxLTUyMGQ0MzMxODM4ZiJ9.eyJ2ZXIiOjksImF1aWQiOiJiZmEwMDEwNTQxZWJmMGJmYTdhNGVmYTcxM2ZmZjI0NCIsImNvZGUiOiI1dFI3YU5iWmlsazV6VDFWNGNIVG5HYWtTb0NtQldwQWciLCJpc3MiOiJ6bTpjaWQ6WEZPVExUcWVSMXlyeDJaV2RNZHp3IiwiZ25vIjowLCJ0eXBlIjoxLCJ0aWQiOjAsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6InM0MnpTVTBjU2lhWGdNay1lUjVXNWciLCJuYmYiOjE2ODkyNzQ0NTcsImV4cCI6MTY5NzA1MDQ1NywiaWF0IjoxNjg5Mjc0NDU3LCJhaWQiOiJSTnZoZkJIZlI4S2V5cXBGQUNKR0FRIn0.i4eBgzmpFD6h4xMNeZPgJAH6Tcvm3jP-9uJQHeQcwVd470sXzFCuIX1ARWgwPVKKCX3OhEh2f1NVGWTZWodqhQ";
        var cID = "XFOTLTqeR1yrx2ZWdMdzw";
        var cSecret = "79scsCkvrCvNOQyZ3Gb8jRUKypdJ3Uk5";

        Console.WriteLine("Enter topic for meeting: ");
        var meetingTopic = Console.ReadLine();

        Console.WriteLine("Enter meeting password: ");
        var meetingPassword = Console.ReadLine();

        Console.WriteLine("Enter start time in this format (YYYY-MM-DDTHH:MM:SSZ): ");
        var meetingStart = Console.ReadLine();

        Console.WriteLine("Enter meeting duration: ");
        int meetingDuration = Convert.ToInt32(Console.ReadLine());

        // Validations
        if (meetingPassword.Length > 10)
        {
            Console.Write("Password cannot have more than 10 characters, please try again!");
            Environment.Exit(0);
        }
        if (!DateTime.TryParse(meetingStart, out _))
        {
            Console.WriteLine("Invalid format of start time please use this format (YYYY-MM-DDTHH:MM:SSZ)");
            Environment.Exit(0);
        }

        refToken = await RefreshAccessTokenAsync(refToken, cID, cSecret);
        await GetMeetings(refToken);
        Tuple<string, string> testResult = await PostMeetings(refToken, meetingTopic, meetingPassword, meetingStart, meetingDuration);

        Console.WriteLine("join URL is: " + testResult.Item1);
        Console.WriteLine("start URL is: " + testResult.Item2);
    }

    static async Task<string> GetMeetings(string refToken)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("https://api.zoom.us/v2/users/me/meetings"),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Authorization", $"Bearer {refToken}");

        var result = await SendHttpRequest(request);
        return result.Item2;
    }

    static async Task<Tuple<string, string>> PostMeetings(string token, string top, string pass, string start, int dur)
    {
        var meetings = new
        {
            topic = top,
            type = 2,
            start_time = start,
            duration = dur,
            timezone = "UTC",
            password = pass
        };

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("https://api.zoom.us/v2/users/me/meetings"),
            Method = HttpMethod.Post
        };
        request.Headers.Add("Authorization", $"Bearer {token}");
        string jsonString = JsonConvert.SerializeObject(meetings);
        request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var result = await SendHttpRequest(request);
        if (result.Item1.IsSuccessStatusCode)
        {
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Item2);
            return Tuple.Create(
                jsonResponse["join_url"].ToString(),
                jsonResponse["start_url"].ToString());
        }
        else
        {
            Console.WriteLine($"Request failed with status code: {result.Item1}");
            return Tuple.Create("", "");
        }
    }

    static async Task<string> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://zoom.us/oauth/token");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"))
        );

        var result = await SendHttpRequest(request);
        if (result.Item1.IsSuccessStatusCode)
        {
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result.Item2);
            return tokenResponse.access_token;
        }
        else
        {
            throw new Exception("Failed to refresh token");
        }
    }

    static async Task<Tuple<System.Net.HttpStatusCode, string>> SendHttpRequest(HttpRequestMessage request)
    {
        try
        {
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return Tuple.Create(response.StatusCode, response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return Tuple.Create(response.StatusCode, "");
            }
        }
        catch (Exception e)
        {
            return Tuple.Create(System.Net.HttpStatusCode.NotFound, "");
        }
    }
}

public class TokenResponse
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string token_type { get; set; }
    public string expires_in { get; set; }
    public string scope { get; set; }
}
