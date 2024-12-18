using UnityEngine;

namespace BehaviourTreeGeneric.NodeSample
{
    public class ActionCreatJob : BAction
    {
        private float m_ftime;
        public override void Enter(BContext input)
        {
            //开始创建任务
            Debug.Log("▶▶▶Start CreatJob");
            this.m_ftime = Time.time;
        }

        public override ActionResult Trick(BContext input)
        {
            Debug.Log("ing");
            if (Time.time - this.m_ftime > 5f)
            {
                //任务创建完成了
                Debug.Log("▶▶▶CreatJob finished");
                return ActionResult.SUCCESS;
            }

            return ActionResult.RUNNING;
        }
    }

}