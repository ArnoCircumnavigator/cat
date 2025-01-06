﻿using System.Collections.Generic;

namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 复合节点，有些地方也称“控制节点”
    /// </summary>
    public class BComposite : BNode
    {
        /// <summary>
        /// 当前索引
        /// </summary>
        protected int _index;

        /// <summary>
        /// 直接子节点
        /// </summary>
        protected List<BNode> _children = new List<BNode>();

        public BComposite()
            : base()
        {
            name = "Composite";
        }

        public override void Enter(BContext bcontext)
        {
            _index = 0;
        }

        public int GetChildCount()
        {
            return this._children.Count;
        }

        public void RemoveChild(BNode node)
        {
            this._children.Remove(node);
        }

        public void AddChild(BNode node)
        {
            this._children.Add(node);
        }

        public void InsertChild(int index, BNode node)
        {
            this._children.Insert(index, node);
        }

        public void ReplaceChild(int index, BNode node)
        {
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