using UnityEngine;

namespace Util
{
    /// <summary>
    /// 规划一件事等多久之后做、做多少次
    /// </summary>
    public class ScheduleData
    {
        private float waitTime;
        private float leftTime;
        
        private int leftActionTime;

        // leftActionTime = -1 表示无限次
        public ScheduleData(float waitTime, int leftActionTime)
        {
            this.waitTime = waitTime;
            this.leftActionTime = leftActionTime;
            leftTime = waitTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>倒计时是否结束</returns>
        public bool Elapse()
        {
            leftTime -= Time.deltaTime;
            return leftTime <= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>是否还有剩余行动次数</returns>
        public bool SubOneTime()
        {
            leftTime = waitTime;
            
            if (leftActionTime < 0)
            {
                return true;
            }

            leftActionTime--;
            return leftActionTime > 0;
        }
    }
}