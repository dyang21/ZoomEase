using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace YourNamespace
{
    [ApiController]
    [Route("[controller]")]
    public class ZoomController : ControllerBase
    {
        private readonly Program _program;

        public ZoomController()
        {
            _program = new Program();
        }

        [HttpGet("refreshToken")]
        public async Task<string> RefreshToken(string refreshToken, string clientId, string clientSecret)
        {
            return await _program.RefreshAccessTokenAsync(refreshToken, clientId, clientSecret);
        }

        [HttpGet("getMeetings")]
        public async Task<string> GetMeetings(string refToken)
        {
            return await _program.GetMeetings(refToken);
        }

        [HttpPost("postMeetings")]
        public async Task<Dictionary<string, string>> PostMeetings([FromBody] Meeting meeting)
        {
            var result = await _program.PostMeetings(meeting.Token, meeting.Top, meeting.Pass, meeting.Start, meeting.Dur);
            return new Dictionary<string, string>
            {
                { "joinURL", result.Item1 },
                { "startURL", result.Item2 }
            };
        }
    }

    public class Meeting
    {
        public string Token { get; set; }
        public string Top { get; set; }
        public string Pass { get; set; }
        public string Start { get; set; }
        public int Dur { get; set; }
    }
}
