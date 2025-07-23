using System;
using System.Collections.Generic;

namespace net5_10_14.Base
{
    public class ActionStack
    {
        private static Dictionary<string, Func<object, bool>> actions = new Dictionary<string, Func<object, bool>>();
        public static void Register(string key, Func<object, bool> action)
        {
            actions.Add(key, action);
        }

        public static bool Execute(string key, object param)
        {
            return actions[key].Invoke(param);
        }
    }
}
