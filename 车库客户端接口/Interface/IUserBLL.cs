using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace 车库客户端接口.Interface
{
    //得到一个用户的详细信息，或者得到所有用户的详细信息..
    public interface IUserBLL
    {
        Task<List<string>> GetAll();
    }
}
