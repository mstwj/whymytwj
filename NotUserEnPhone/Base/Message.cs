using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotUserEnPhone.Base
{
    public class Message
    {
        static Dictionary<string, Action<string, string, string>> actions = new Dictionary<string, Action<string, string, string>>();

        public static void Register(Action<string, string, string> action, string key)
        {
            if (!actions.ContainsKey(key))
            {
                actions.Add(key, action);
            }
        }

        public static void Show(string key, string title, string msg, string btn)
        {
            if (actions.ContainsKey(key))
            {
                actions[key]?.Invoke(title, msg, btn);
            }
        }
    }
}
