using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BehaviourTreeGeneric
{
    public class BNode
    {
        protected string type;
        protected string name;
        protected BNode _parent; //直接父节点
        protected List<BNode> _children = new List<BNode>(); //直接子节点
        ActionResult _state = ActionResult.NONE;

        protected BNode()
        {
            this.type = GetType().FullName;
            this.name = GetType().Name;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public unsafe ActionResult Execute(BInput context)
        {
            /*
             * 常见的行为节点只有一个Trick，
             * 这里包一层，多提供enter和exit两个时机
             */
            if (_state == ActionResult.NONE)
            {
                Enter(context);
                _state = ActionResult.RUNNING;
            }
            var result = Trick(context);
            if (result != ActionResult.RUNNING)//没有进入运行中，就重置，下周期重新进入
            {
                Exit(context);
                _state = ActionResult.NONE;
            }
            return result;
        }

        public virtual unsafe void Enter(BInput input) { }
        public virtual unsafe ActionResult Trick(BInput input) { return ActionResult.SUCCESS; }
        public virtual unsafe void Exit(BInput input) { }

        public void RemoveChild(BNode node)
        {
            this._children.Remove(node);
        }
        public void AddChild(BNode node)
        {
            this._children.Add(node);
        }
        public void InsertChild(BNode prenode, BNode node)
        {
            int index = this._children.FindIndex((a) => { return a == prenode; });
            this._children.Insert(index, node);
        }
        public void ReplaceChild(BNode prenode, BNode node)
        {
            int index = this._children.FindIndex((a) => { return a == prenode; });
            this._children[index] = node;
        }
        public bool ContainChild(BNode node)
        {
            return this._children.Contains(node);
        }

        internal BNode[] GetBNodesCopy()
        {
            BNode[] nds = new BNode[this._children.Count];
            _children.CopyTo(nds, 0);
            return nds;
        }

        public override string ToString()
        {
            //return $"{name}[{type}]";
            return $"[{name}]";
        }

        public void Load(JToken json)
        {
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

            for (int i = 0; i < json["child"].Count(); i++)
            {
                string typename = json["child"][i]["type"].ToString();
                Type chile_type = Type.GetType(typename);
                BNode enode = Activator.CreateInstance(chile_type) as BNode;
                if (enode == null)
                    throw new Exception($"节点无法实例化{typename}，请确保程序集正确");
                enode.Load(json["child"][i]);//子节点读取
                enode._parent = this;//设置其父节点为自身
                this.AddChild(enode);
            }
        }
    }
}
