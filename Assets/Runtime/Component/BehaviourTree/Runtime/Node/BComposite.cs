namespace BehaviourTreeGeneric
{
    /// <summary>
    /// 复合节点
    /// </summary>
    public class BComposite : BNode
    {
        protected int _index;

        public BComposite()
            : base()
        {
            name = "Composite";
        }

        public override void Enter(BInput input)
        {
            _index = 0;
        }
    }
}