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

        public override ActionResult Trick(BInput input)
        {
            UnityEngine.Debug.Log($"{this} tricking");

            if (this._index >= this._children.Count)
            {
                return ActionResult.SUCCESS;
            }

            BNode node = this._children[this._index];

            ActionResult res = node.Execute(input);

            if (res == ActionResult.FAILURE)
                return ActionResult.FAILURE;

            if (res == ActionResult.SUCCESS)
            {
                this._index++;
            }

            return ActionResult.RUNNING;
        }
    }
}