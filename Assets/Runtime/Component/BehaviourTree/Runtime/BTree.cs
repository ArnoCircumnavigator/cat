using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace BehaviourTreeGeneric
{
    public class BTree
    {
        string name;

        /// <summary>
        /// 根部节点
        /// </summary>
        BNode root;

        /*
         * 在物流中，很少有“正在运行中的行为可以自然地中止切换到其他行为”这种情况
         * 所以设计成，每次trick结束时，如果状态时正在进行中，则记录位置，下次trick时，从上次离开的地方重新进入
         * 但是！不知道怎么做，尝试过的方法，副作用都很大
         */

        public BTree() { }

        public ActionResult Run(BContext input)
        {
            return root.Execute(input);
        }

        public void Load(string treePersistence)
        {
            JObject json = JObject.Parse(treePersistence);
            name = json["name"].ToString();
            root = null;
            if (json.ContainsKey("node"))
            {
                string typename = json["node"]["type"].ToString();
                Type t = Type.GetType(typename);
                root = Activator.CreateInstance(t) as BNode;
                if (root == null)
                    throw new Exception($"行为树无法实例化{typename}，请确保程序集正确");
                root.Load(json["node"].ToString());
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{name}");

            void recursion(BNode node, int deep)
            {
                sb.AppendLine($"{new string(' ', deep * 4)}{node.ToString()}");
                if(node is BComposite bComposite)
                {
                    foreach (BNode c in bComposite.GetChildrenCopy())
                        recursion(c, deep++);
                }
            }

            recursion(root, 1);
            return sb.ToString();
        }
    }
}