namespace BehaviourTreeGeneric.NodeSample
{
    public class ConditionEmpty : BCondition
    {
        public override ActionResult Trick(BInput input)
        {
            if (!input.hasCargo)
            {
                UnityEngine.Debug.Log("▶▶▶为空 成立");
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }
}