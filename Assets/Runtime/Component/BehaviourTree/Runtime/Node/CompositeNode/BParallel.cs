namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 平行节点
    /// </summary>
    public class BParallel : BComposite
    {
        public BParallel()
            :base()
        {
            name = "Parallel";
        }

        public override ActionResult Trick(BContext bcontext)
        {
            //平行节点的完成条件
            if (_index >= _children.Count)
                return ActionResult.SUCCESS;

            BNode node = _children[_index];

            ActionResult result = node.Execute(bcontext);

            if (result != ActionResult.RUNNING)
            {
                _index++;
                //检查一次完成
                if (_index >= _children.Count)
                    return ActionResult.SUCCESS;
            }

            return ActionResult.RUNNING;
        }
    }
}