using System;
using System.Globalization;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

public class Program
{
    static readonly HttpClient client = new HttpClient();
    static async Task Main(string[] args)
    {
        // Refresh Token from PostMan
        var refToken = "eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjU4OTk1MzNiLTZhYjAtNDNlYi1hNjQxLTUyMGQ0MzMxODM4ZiJ9.eyJ2ZXIiOjksImF1aWQiOiJiZmEwMDEwNTQxZWJmMGJmYTdhNGVmYTcxM2ZmZjI0NCIsImNvZGUiOiI1dFI3YU5iWmlsazV6VDFWNGNIVG5HYWtTb0NtQldwQWciLCJpc3MiOiJ6bTpjaWQ6WEZPVExUcWVSMXlyeDJaV2RNZHp3IiwiZ25vIjowLCJ0eXBlIjoxLCJ0aWQiOjAsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6InM0MnpTVTBjU2lhWGdNay1lUjVXNWciLCJuYmYiOjE2ODkyNzQ0NTcsImV4cCI6MTY5NzA1MDQ1NywiaWF0IjoxNjg5Mjc0NDU3LCJhaWQiOiJSTnZoZkJIZlI4S2V5cXBGQUNKR0FRIn0.i4eBgzmpFD6h4xMNeZPgJAH6Tcvm3jP-9uJQHeQcwVd470sXzFCuIX1ARWgwPVKKCX3OhEh2f1NVGWTZWodqhQ";
        // Client ID from Zoom App Management Tab
        var cID = "XFOTLTqeR1yrx2ZWdMdzw";
        // Client Secret from Zoom App Management Tab
        var cSecret = "79scsCkvrCvNOQyZ3Gb8jRUKypdJ3Uk5";

        var testRefToken = string.Empty;
        /*
        var meetingTopic = "Customized topic";
        string meetingPassword = "joshua";
        var meetingStart = "2024-04-02T15:59:00Z";
        int meetingDuration = 60;
        */

        // Gets parameters from user
        Console.WriteLine("Enter topic for meeting: ");
        var meetingTopic = Console.ReadLine();

        Console.WriteLine("Enter meeting password: ");
        string meetingPassword = Console.ReadLine();

        Console.WriteLine("Enter start time in this format (YYYY-MM-DDTHH:MM:SSZ): ");
        var meetingStart = Console.ReadLine();

        Console.WriteLine("Enter meeting duration: ");
        string durationTemp = Console.ReadLine();
        int meetingDuration = Convert.ToInt32(durationTemp);


        // Checks if password is proper length
        if (meetingPassword.Length > 10)
        {
            Console.Write("Password cannot have more than 10 characters, please try again!");
            Environment.Exit(0);
        }

        // Checks if start time is properlly formatted
        string pattern = @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z";

        if(!Regex.IsMatch(meetingStart, pattern))
        {
            Console.WriteLine("Invalid format of start time please use this format (YYYY-MM-DDTHH:MM:SSZ)");
            Environment.Exit(0);
        }


        // Gets access token first
        testRefToken = await RefreshAccessTokenAsync(refToken, cID, cSecret);
        await GetMeetings(testRefToken);
        Tuple<string, string> testResult = await PostMeetings(testRefToken, meetingTopic, meetingPassword, meetingStart, meetingDuration);
        string joinURL = testResult.Item1;
        string startURL = testResult.Item2;
        
        // Ouputs the meeting links
        Console.WriteLine("join URL is: " + joinURL);
        Console.WriteLine("start URL is: " + startURL);

    }


    // Just a method to test the HTTP request and other testing
    static async Task<string> GetMeetings(string refToken)
    {
        string accessToken = refToken;

        // Empty place holder for return value
        string responseString = string.Empty;

        using (var client = new HttpClient())
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://api.zoom.us/v2/users/me/meetings");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                responseString = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        return responseString;
    }

    // Main method that returns two variables, one meeting link for guest and one meeting link for host
    static async Task<Tuple<string, string>> PostMeetings(string token, string top, string pass, string start, int dur)
    {


        string accessToken = token;
        string respString = string.Empty;
        var joinZoom = string.Empty;
        var startZoom = string.Empty;
        var date = string.Empty;

        // Makes use of "using" in order to properly dispose of data/memory
        using (var client = new HttpClient())
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://api.zoom.us/v2/users/me/meetings");
            request.Method = HttpMethod.Post;
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            try
            {
                var meetings = new
                {
                    // Changing the meeting properties based off of the parameters passed by the user in the main
                    topic = top,
                    type = 2,
                    start_time = start,
                    duration = dur,
                    timezone = "UTC",
                    password = pass
                };
                string jsonString = JsonConvert.SerializeObject(meetings);
                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                respString = await response.Content.ReadAsStringAsync();

                // Get the join_url
                if(response.IsSuccessStatusCode)
                {
                    // Converting the date time format to a proper format in order to check that the date time is valid 
                    var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(respString);
                    date = jsonResponse["start_time"].ToString();
                    DateTime dateTime = DateTime.Parse(start, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                    string newFormat = dateTime.ToString("M/d/yyyy h:mm:ss tt");

                    if (date != newFormat)
                    {
                        Console.WriteLine("The date you have entered is invalid, please only input dates that are in the future! Please try again!");
                        Environment.Exit(0);
                    }

                    jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(respString);
                    if(jsonResponse.ContainsKey("join_url"))
                    {
                        joinZoom = jsonResponse["join_url"].ToString();
                        startZoom = jsonResponse["start_url"].ToString();
                    }
                    else
                    {
                        Console.WriteLine("The 'join_url' was not found in the response");
                    }

                    
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return Tuple.Create(joinZoom, startZoom);
    }
    
    // Method to automatically grab the access token, no need to manually change it
    static async Task<string> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret)
    {
        using (var client = new HttpClient())
        {
            var tokenEndpnt = "https://zoom.us/oauth/token";
            var refreshRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpnt);

            var encodedContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            refreshRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"))
            );

            refreshRequest.Content = encodedContent;

            HttpResponseMessage response = await client.SendAsync(refreshRequest);

            if(response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);
                return tokenResponse.access_token;
            }
            else
            {
                throw new Exception("Failed to refresh token");
            }
        }
  
    }
}

public class TokenResponse
{ 
    public string access_token { get; set; }
    public string refresh_token { get; set;}
    public string token_type { get; set;}
    public string expires_in { get; set; }
    public string scope { get; set; }

}



