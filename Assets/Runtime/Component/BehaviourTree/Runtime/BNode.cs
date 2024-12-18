using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace BehaviourTreeGeneric
{
    public unsafe class BNode
    {
        protected string type;
        protected string name;

        internal BNode _parent; //直接父节点

        ActionResult _state = ActionResult.NONE;

        protected BNode()
        {
            this.type = GetType().FullName;
            this.name = GetType().Name;
        }

        /*
         * 标准bt设计中，没有Execute这个东西
         * 我加上的目的上为了把“进入”“离开”这两个时机留下来
         * 一定程度上提高节点对时机的控制自由度
         * 
         * 2024-12-16 张兴留
         */

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="bcontext"></param>
        /// <returns></returns>
        public unsafe ActionResult Execute(BContext bcontext)
        {
            if (_state == ActionResult.NONE)
            {
                Enter(bcontext);
                _state = ActionResult.RUNNING;
            }

            var res = Trick(bcontext);

            if (res != ActionResult.RUNNING)//Trick后，没有进入运行中，即成功或失败，总之是结束了，节点状态改为none
            {
                Exit(bcontext);
                _state = ActionResult.NONE;
            }

            return res;
        }

        public virtual void Enter(BContext input) { }
        public virtual ActionResult Trick(BContext input) { return ActionResult.SUCCESS; }
        public virtual void Exit(BContext input) { }

        public override string ToString()
        {
            //return $"{name}[{type}]";
            return $"[{name}]";
        }

        public void Load(string nodePersistence)
        {
            var json = JToken.Parse(nodePersistence);
            type = json["type"].ToString();
            name = json["name"].ToString();

            var arg = json["arg"];
            Type t = GetType();
            FieldInfo[] fieldInfos = t.GetFields();
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo info = fieldInfos[i];
                if (arg.Contains(info.Name))
                {
                    string str = arg[info.Name].ToString();
                    object value = null;
                    if (info.FieldType == typeof(int)) value = int.Parse(str);
                    else if (info.FieldType == typeof(float)) value = float.Parse(str);
                    else if (info.FieldType == typeof(bool)) value = bool.Parse(str);
                    else if (info.FieldType == typeof(string)) value = str;
                    info.SetValue(this, value);
                }
            }

            if (this is BComposite bComposite)
            {
                for (int i = 0; i < json["child"].Count(); i++)
                {
                    string typename = json["child"][i]["type"].ToString();
                    Type chile_type = Type.GetType(typename);
                    if (!(Activator.CreateInstance(chile_type) is BNode enode))
                        throw new Exception($"节点无法实例化{typename}，请确保程序集正确");
                    enode.Load(json["child"][i].ToString());//子节点读取
                    enode._parent = this;//设置其父节点为自身
                    bComposite.AddChild(enode);
                }
            }
        }
    }
}
