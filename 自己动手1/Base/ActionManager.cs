using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自己动手1.Base
{
    public class ActionManager
    {
        //和事件不一样，事件是可以挂很多，上抛，委托一样..可是不能=+这样..
        // 委托
        //Delegeat 可以接收2种类型的 委托... 有参和无参的..
        static Dictionary<string, Delegate> actionMap = new Dictionary<string, Delegate>();

        // 希望能将View中的行为方法存放进来   注册
        // 由窗口对象调用 ，注册一个方法，这个方法中有打开弹窗的逻辑
        // 优化：由不同实例进行注册的时候  进行区分
        // 这个方法可以让我们接受两种类型的委托对象  Action  Func
        public static void Register(string key, Delegate action)
        {
            if (!actionMap.ContainsKey(key))
                actionMap.Add(key, action);
        }

        // VM中需要进行调用
        public static void Execute(string key, object data)
        {
            if (actionMap.ContainsKey(key))
                actionMap[key].DynamicInvoke(data);
        }

        public static bool ExecuteAndResult(string key, object data)
        {
            if (actionMap.ContainsKey(key))
            {
                var action = (actionMap[key] as Func<object, bool>);
                if (action == null)
                    return false;

                return action.Invoke(data);
            }
            return false;
        }

        public static void Unregister(string key)
        {

        }
    }
}
