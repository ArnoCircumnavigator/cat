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

        public override ActionResult Trick(BInput input)
        {
            UnityEngine.Debug.Log($"\"{this}\" tricking");

            if (_index >= _children.Count)
                return ActionResult.SUCCESS;

            BNode node = _children[_index];

            var result = node.Execute(input);

            if (result != ActionResult.RUNNING)
                _index++;

            return ActionResult.RUNNING;
        }
    }
}