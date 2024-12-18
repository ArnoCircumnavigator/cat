namespace BehaviourTreeGeneric.NodeSample
{
    public class ConditionEmpty : BCondition
    {
        public override ActionResult Trick(BContext input)
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