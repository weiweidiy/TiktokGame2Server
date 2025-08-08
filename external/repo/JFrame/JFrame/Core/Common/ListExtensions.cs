using System;
using System.Collections.Generic;

namespace JFramework
{
    public static class ListExtensions
    {
        /// <summary>
        /// 获取列表中的随机元素（支持无放回和有放回）
        /// </summary>
        public static List<T> GetRandomItems<T>(this List<T> list, int count)
        {
            if (list == null || list.Count == 0 || count <= 0)
                return new List<T>();

            List<T> result = new List<T>(count);
            Random random = new Random();

            if (count <= list.Count)
            {
                // 无放回抽取
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
                // 有放回抽取
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