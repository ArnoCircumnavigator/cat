namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 选择节点
    /// </summary>
    public class BSelector : BComposite
    {
        public BSelector()
            :base()
        {
            name = "Selector";
        }

        public override ActionResult Trick(BContext bcontext)
        {
            //选择节点的完成条件
            if (_index >= _children.Count)
                return ActionResult.FAILURE;

            BNode node = _children[_index];
            ActionResult result = node.Execute(bcontext);

            if (result == ActionResult.SUCCESS)
                return ActionResult.SUCCESS;

            if (result == ActionResult.FAILURE)
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