namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 执行节点
    /// </summary>
    public class BAction : BNode
    {
        public BAction()
            : base()
        {
            name = "Action";
        }

        public override ActionResult Trick(BInput input)
        {
            UnityEngine.Debug.Log($"\"{this}\" tricking");
            return ActionResult.SUCCESS;
        }
    }
}