using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 童文君第一个Prism项目.Interface
{
    public interface ICommunication
    {
        public event EventHandler<byte[]> Receiveved;
        bool Connect(string host, string port);
    }
}
