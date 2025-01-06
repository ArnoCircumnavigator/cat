using Cat;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using static BehaviourTreeGeneric.Literal;

namespace BehaviourTreeGeneric
{
    public class BTree
    {
        public string name;

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

        static readonly ILoger Loger = new Loger("btree");
        public ActionResult Run(BContext input)
        {
            return root.Execute(input);
        }

        public void Load(JToken json)
        {
            try
            {
                name = json[NAME].ToString();
                JToken json_node = json[NODE];
                if (json_node == null)
                    return;

                //根
                root = (BNode)Activator.CreateInstance(Type.GetType(json_node[TYPE].ToString()));

                //递归，完成反序列化
                DeserializeWithRecursion(root, json_node);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"反序列化异常 {ex}");
            }

            static void DeserializeWithRecursion(BNode parent, JToken json_node)
            {
                if (json_node == null)
                    return;
                parent.Deserialize(json_node);
                foreach (JToken cj in json_node[CHILD])
                {
                    BNode bnode = (BNode)Activator.CreateInstance(Type.GetType(cj[TYPE].ToString()));
                    if (parent is BComposite composite)
                        composite.AddChild(bnode);
                    DeserializeWithRecursion(bnode, cj);
                }
            }
        }

        //序列化回json文本
        public JToken Serialize()
        {
            JObject json = new JObject();
            json[NAME] = name;//树的名字
            JObject jnode = new JObject();
            Serialize(jnode, root);
            json[NODE] = jnode;
            return json;

            static void Serialize(JToken j, BNode b)
            {
                JToken nodejson = b.Serialize();
                Loger.Log(nodejson);
                //j = nodejson;

                if (b is BComposite composite)
                {
                    var childCount = composite.GetChildCount();
                    for (int i = 0; i < childCount; i++)
                    {
                        Serialize(nodejson, composite[i]);
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{name}");

            void recursion(BNode node, int deep)
            {
                sb.AppendLine($"{new string(' ', deep * 4)}{node.ToString()}");
                if (node is BComposite bComposite)
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