namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 顺序节点
    /// </summary>
    public class BSequence : BComposite
    {
        public BSequence()
            : base()
        {
            name = "Sequence";
        }

        public override ActionResult Trick(BContext bcontext)
        {
            //顺序节点的完成条件
            if (this._index >= this._children.Count)
                return ActionResult.SUCCESS;

            BNode node = this._children[this._index];
            ActionResult result = node.Execute(bcontext);

            if (result == ActionResult.FAILURE)
                return ActionResult.FAILURE;

            if (result == ActionResult.SUCCESS)
            {
                this._index++;
                //检查一次完成
                if (_index >= _children.Count)
                    return ActionResult.SUCCESS;
            }
            return ActionResult.RUNNING;
        }
    }
}