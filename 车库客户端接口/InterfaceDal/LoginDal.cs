using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using 车库客户端接口.Interface;

namespace 车库客户端接口.InterfaceDal
{
    public class LoginDal : WebDataAccess, ILoginDal
    {
        public Task<string> Login(string username, string password)
        {
            Dictionary<string, HttpContent> contents = new Dictionary<string, HttpContent>();
            contents.Add("username", new StringContent(username));
            contents.Add("password", new StringContent(password));

            return this.PostDatas("User/login", contents);
        }
    }
}
