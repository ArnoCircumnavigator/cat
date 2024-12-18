/*
 * 装饰器节点，有且仅有一个孩子
 */

using System;

namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 装饰器节点
    /// </summary>
    public class BDecorator : BNode
    {
        /// <summary>
        /// 装饰节点的唯一孩子节点
        /// 必须不为空
        /// </summary>
        protected BNode _child;

        /// <summary>
        /// 装饰节点
        /// </summary>
        public BDecorator(BNode child)
            : base()
        {
            _child = child ?? throw new Exception("装饰器节点的孩子必不为空");
            name = "Decorator";
        }
    }
}