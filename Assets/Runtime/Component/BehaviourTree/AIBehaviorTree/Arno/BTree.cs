using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;
using static PlasticGui.LaunchDiffParameters;

namespace BehaviourTreeGeneric
{
    public class BTree
    {
        string name;

        BNode root;

        public BTree() { }

        public void Run(BInput input)
        {
            root.Trick(input);
        }

        public void Load(JObject json)
        {
            name = json["name"].ToString();
            root = null;
            if (json.ContainsKey("node"))
            {
                string typename = json["node"]["type"].ToString();
                Type t = Type.GetType(typename);
                root = Activator.CreateInstance(t) as BNode;
                if (root == null)
                    throw new Exception($"行为树无法实例化{typename}，请确保程序集正确");
                root.Load(json["node"]);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{name}");

            void recursion(BNode node, int deep)
            {
                sb.AppendLine($"{new string(' ', deep * 4)}{node.ToString()}");
                foreach (BNode c in node.GetBNodesCopy())
                    recursion(c, deep++);
            }

            recursion(root, 1);
            return sb.ToString();
        }
    }
}