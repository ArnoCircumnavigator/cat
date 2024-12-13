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

        public override ActionResult Trick(BInput input)
        {
            UnityEngine.Debug.Log($"\"{this}\" tricking");
            if (_index >= _children.Count)
            {
                return ActionResult.FAILURE;
            }

            BNode node = _children[_index];

            ActionResult res = node.Execute(input);

            if (res == ActionResult.SUCCESS)
                return ActionResult.SUCCESS;

            if (res == ActionResult.FAILURE)
            {
                this._index++;
            }
            return ActionResult.RUNNING;
        }
    }
}