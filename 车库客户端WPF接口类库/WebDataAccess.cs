using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace 车库客户端WPF接口类库
{
    public class WebDataAccess
    {
        public string domain = "http://localhost:6687/api/";

        public Task<string> GetDatas(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                var resp = client.GetAsync($"{domain}{uri}").GetAwaiter().GetResult();
                return resp.Content.ReadAsStringAsync();
            }
        }

        private MultipartFormDataContent GetFormData(Dictionary<string, HttpContent> contents)
        {
            var postContent = new MultipartFormDataContent();
            string boundary = $"-------{DateTime.Now.Ticks.ToString("x")}-----------";
            postContent.Headers.Add("ContentType", $"muiltipart/form-data,boundary={boundary}");

            foreach (var item in contents)
            {
                postContent.Add(item.Value, item.Key);
            }

            return postContent;
        }

        public Task<string> PostDatas(string uri, Dictionary<string, HttpContent> contents)
        {
            using (HttpClient client = new HttpClient())
            {
                var resp = client.PostAsync($"{domain}{uri}", this.GetFormData(contents)).GetAwaiter().GetResult();
                return resp.Content.ReadAsStringAsync();
            }
        }

    }
}
