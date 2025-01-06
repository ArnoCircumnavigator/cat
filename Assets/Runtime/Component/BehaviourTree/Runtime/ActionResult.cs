namespace BehaviourTreeGeneric
{
    public enum ActionResult
    {
        /// <summary>
        /// 可以理解为初始状态，这个状态，下次进入该节点，会执行Enter()
        /// </summary>
        NONE,
        SUCCESS,
        RUNNING,
        FAILURE
    }
}