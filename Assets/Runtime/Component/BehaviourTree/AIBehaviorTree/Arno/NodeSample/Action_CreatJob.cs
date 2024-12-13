using UnityEngine;

namespace BehaviourTreeGeneric.NodeSample
{
    public class ActionCreatJob : BAction
    {
        private float m_ftime;
        public override void Enter(BInput input)
        {
            //开始创建任务
            Debug.Log("▶▶▶CreatJob");
            this.m_ftime = Time.time;
        }

        public override ActionResult Trick(BInput input)
        {
            if (Time.time - this.m_ftime > 2f)
            {
                //任务创建完成了
                Debug.Log("▶▶▶CreatJob finished");
                return ActionResult.SUCCESS;
            }

            return ActionResult.RUNNING;
        }
    }

}