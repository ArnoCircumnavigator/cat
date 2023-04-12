using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.Memo
{
    public class CatRandom
    {
        System.Random Random;
        public CatRandom()
        {
            Random = new System.Random();
        }
        /// <summary>
        /// 随机命中
        /// </summary>
        /// <param name="weights">权重</param>
        /// <returns></returns>
        public int GetRandom(params int[] weights)
        {
            int length = weights.Length;

            double sum = 0;
            for (int i = 0; i < length; i++) { sum += weights[i]; }

            var normalized = new double[length];
            for (int i = 0; i < length; i++)
            {
                double _n = weights[i] / sum;
                normalized[i] = i != 0 ? _n + normalized[i - 1] : _n;
            }

            var next = Random.NextDouble();
            for (int i = length - 1; i >= 0; i--)
            {
                if (normalized[i] < next) { return i + 1; }
            }
            return 0;
        }

        [Obsolete]
        /// <summary>
        /// 随机命中
        /// </summary>
        /// <param name="weights">权重</param>
        /// <returns>参数数组中的索引</returns>
        public int GetRandomDebug(params int[] weights)
        {
            //范围是[0-100)
            double start = 0;
            double range = 1;
            //参与随机的角色数量
            var count = weights.Length;
            float totalWeight = weights.Sum();

            //各角色的比重
            float[] ratios = new float[count];
            for (int i = 0; i < count; i++)
            {
                ratios[i] = weights[i] / totalWeight;
            }
            //根据比重，将【0,range)切开
            Section[] sections = new Section[count];
            for (int i = 0; i < count; i++)
            {
                var s = new Section();
                s.Left = start;
                s.LeftIsOpen = false;//左侧闭区间
                s.Right = start + ratios[i] * range;
                s.RightIsOpen = false;//右侧开区间
                //if (i == count - 1)
                //    s.RightIsOpen = false;//最后一个也是闭区间
                sections[i] = s;
                start = s.Right;
            }
            #region 打印区间
            //foreach (var s in sections)
            //{
            //    Console.WriteLine(s.ToString() + "\n");
            //}
            #endregion
            //创建一个随机数
            var randomValue = Random.NextDouble();
            #region 打印随机数
            //Console.WriteLine("random : " + randomValue);
            #endregion
            //随机数命中的区间
            var section = sections.Where(p => p.IsCover(randomValue)).FirstOrDefault();
            //当前区间的索引（+1就是传入参数哪些权重的顺序）
            var index = Array.IndexOf(sections, section);
            if (index == -1)
                throw new System.Exception("无法命中");
            return index;
        }
        [Obsolete]
        /// <summary>
        /// 一个浮点数型的区间
        /// </summary>
        struct Section
        {
            /// <summary>
            /// 左边界
            /// </summary>
            public double Left;
            /// <summary>
            /// 左边界是否开区间
            /// </summary>
            public bool LeftIsOpen;
            /// <summary>
            /// 右边界
            /// </summary>
            public double Right;
            /// <summary>
            /// 右边界是否开区间
            /// </summary>
            public bool RightIsOpen;
            public bool IsCover(double value)
            {
                var cdLeft = LeftIsOpen ? value > Left : value >= Left;
                var cdRight = RightIsOpen ? value < Right : value <= Right;
                return cdLeft && cdRight;
            }
            public override string ToString()
            {
                string sleft = LeftIsOpen ? "(" : "[";
                string sright = RightIsOpen ? ")" : "]";
                return sleft + Left + "," + Right + sright;
            }
        }
    }
}
