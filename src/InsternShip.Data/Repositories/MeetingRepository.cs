using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Web;
using System.Text.Json.Nodes;
using Azure.Core;
using System.Runtime.Intrinsics.X86;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Newtonsoft.Json.Linq;
using CloudinaryDotNet;
using Org.BouncyCastle.Asn1.Ocsp;

namespace InsternShip.Data.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly IConfiguration _config;
        public MeetingRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> GetZoomAccessToken()
        {
            
            string clientId = _config.GetValue<string>("ZoomClientId");
            string clientSecret = _config.GetValue<string>("ZoomClientSecret");

            // Replace these with the URL to obtain an access token
            string accId = _config.GetValue<string>("ZoomAccountId");
            string tokenEndpoint = "https://zoom.us/oauth/token" + "?grant_type=account_credentials&account_id=" + accId;

            //string code = await WebexAuthorize(clientId);
            var payload = new
            {
                grant_type = "account_credentials",
                account_id = accId,
            };

            string jsonObject = JsonConvert.SerializeObject(payload);
            //string url = $"{tokenEndpoint}?grant_type={grantType}&client_id={clientId}&client_secret={clientSecret}&redirect_uri={redirectUri}";
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            //var result = client.PostAsync(url, content).Result;
            using (var httpClient = new HttpClient())
            {
                // HttpResponseMessage response = await httpClient.GetAsync(url);
                string basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuth}");


                var response = await httpClient.PostAsync(tokenEndpoint, content);

                var json = await response.Content.ReadAsStringAsync();
                var value = JObject.Parse(json);
                string objectId = (string)value["access_token"]; 
                return objectId;
            }
        }
        public async Task<MeetingModel> CreateZoomMeeting(CreateMeetingModel request)
        {
            //string webexAccessToken = await GetAccessToken(clientId, clientSecret, code);
            string accessToken = await GetZoomAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                // Prepare the API request payload
                string apiEndpoint = "https://api.zoom.us/v2/users/me/meetings";

                /*string requestBody = $@"
                {{
                    ""topic"": ""{title}"",
                    ""type"": 2,
                    ""start_time"": ""2019-06-14T10:21:57"",
                    ""duration"": ""45"",
                    ""timezone"": ""Europe/Madrid"",
                    ""agenda"": ""test"",
                    ""recurrence"": {{
                        ""type"": 1,
                        ""repeat_interval"": 1 
                    }},
                    ""settings"": {{
                        ""host_video"": ""true"",
                        ""participant_video"": ""true"",
                        ""join_before_host"": ""False"",
                        ""mute_upon_entry"": ""False"",
                        ""watermark"": ""true"",
                        ""audio"": ""voip"",
                        ""auto_recording"": ""cloud""
                    }}
                }}";*/
                string json = JsonConvert.SerializeObject(new
                {
                    topic = request.Title,
                    start_time = request.StartTime,
                    duration = 45,
                    timezone = "Asia/Saigon"
                });
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    //string jsonObject = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(apiEndpoint, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var value = JObject.Parse(responseContent);
                        
                        return new MeetingModel {
                            MeetingId = (string)value["id"],
                            Title = request.Title,
                            StartUrl = (string)value["start_url"],
                            JoinUrl = (string)value["join_url"],
                            Time = DateTime.Parse((string)value["start_time"]),
                            Duration = (int)value["duration"]
                        }; // Return the API response if successful
                    }
                    else
                    {
                        throw new Exception($"Error creating meeting: {response.ReasonPhrase}");
                    }
                }
            }
            else
            {
                throw new Exception("Unable to obtain Zoom access token.");
            }
        }
        public async Task<object> GetAll(string? type, int? limit, int? page)
        {
            page = page != 0 ? page : 1;
            limit = limit != 0 ? limit : 10;
            type = string.IsNullOrEmpty(type) ? "scheduled" : type.ToLower();
            if (type != "scheduled" && type != "live" && type != "upcoming" && type != " upcoming_meetings " && type != " previous_meetings")
                type = "scheduled";
            string accessToken = await GetZoomAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                string apiEndpoint = "https://api.zoom.us/v2/users/me/meetings" + "?type=" + type + "&page_size=" + limit + "&page_number=" + page;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    var response = await httpClient.GetAsync(apiEndpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var value = JObject.Parse(responseContent);
                        var list = new MeetingListModel
                        {
                            //NextPageToken = (string)value["next_page_token"],
                            TotalCount = (int)value["total_records"],
                            Meetings = new List<MeetingBaseModel>()
                        };
                        foreach (var item in value["meetings"])
                        {
                            list.Meetings.Add(new MeetingBaseModel
                            {
                                MeetingId = (string)item["id"],
                                Title = (string)item["topic"],
                                JoinUrl = (string)item["join_url"],
                                Duration = (int)item["duration"],
                                Time = (DateTime)item["start_time"]
                            });
                        }    
                        //dynamic json = JsonConvert.DeserializeObject<object>(responseContent);

                        return list;
                    }
                    else
                    {
                        throw new Exception($"Error creating meeting: {response.ReasonPhrase}");
                    }
                }
            }
            else
            {
                throw new Exception("Unable to obtain Zoom access token.");
            }
        }
        public async Task<object> GetById(string id)
        {
            string accessToken = await GetZoomAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                string apiEndpoint = "https://api.zoom.us/v2/meetings/" + id;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    var response = await httpClient.GetAsync(apiEndpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var value = JObject.Parse(responseContent);

                        return new MeetingModel
                        {
                            MeetingId = (string)value["id"],
                            Title = (string)value["topic"],
                            StartUrl = (string)value["start_url"],
                            JoinUrl = (string)value["join_url"],
                            Time = DateTime.Parse((string)value["start_time"]),
                            Duration = (int)value["duration"]
                        }; // Return the API response if successful
                    }
                    else
                    {
                        throw new Exception($"Error getting meeting: {response.ReasonPhrase}");
                    }
                }
            }
            else
            {
                throw new Exception("Unable to obtain Zoom access token.");
            }
        }
        public async Task<bool> Delete(string id)
        {
            string accessToken = await GetZoomAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                string apiEndpoint = "https://api.zoom.us/v2/meetings/" + id;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    var response = await httpClient.DeleteAsync(apiEndpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception($"Error deleting meeting: {response.ReasonPhrase}");
                    }
                }
            }
            else
            {
                throw new Exception("Unable to obtain Zoom access token.");
            }
        }
    }
    
}
