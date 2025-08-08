using System;
using System.Collections.Generic;

namespace JFramework
{
    public static class ListExtensions
    {
        /// <summary>
        /// ��ȡ�б��е����Ԫ�أ�֧���޷Żغ��зŻأ�
        /// </summary>
        public static List<T> GetRandomItems<T>(this List<T> list, int count)
        {
            if (list == null || list.Count == 0 || count <= 0)
                return new List<T>();

            List<T> result = new List<T>(count);
            Random random = new Random();

            if (count <= list.Count)
            {
                // �޷Żس�ȡ
                List<T> tempList = new List<T>(list);
                for (int i = 0; i < count; i++)
                {
                    int index = random.Next(tempList.Count);
                    result.Add(tempList[index]);
                    tempList.RemoveAt(index);
                }
            }
            else
            {
                // �зŻس�ȡ
                for (int i = 0; i < count; i++)
                {
                    int index = random.Next(list.Count);
                    result.Add(list[index]);
                }
            }

            return result;
        }
    }
}