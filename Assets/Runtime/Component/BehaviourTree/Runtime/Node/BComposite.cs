using System.Collections.Generic;

namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 复合节点
    /// </summary>
    public class BComposite : BNode
    {
        protected int _index;
        protected List<BNode> _children = new List<BNode>(); //直接子节点

        public BComposite()
            : base()
        {
            name = "Composite";
        }

        public override void Enter(BContext bcontext)
        {
            _index = 0;
        }

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

        internal BNode this[int index]
        {
            get { return this._children[index]; }
        }

        internal BNode[] GetChildrenCopy()
        {
            BNode[] nds = new BNode[this._children.Count];
            _children.CopyTo(nds, 0);
            return nds;
        }
    }
}