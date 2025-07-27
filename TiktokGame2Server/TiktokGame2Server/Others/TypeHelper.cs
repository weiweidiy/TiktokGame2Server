using System;
using System.Linq;
using System.Reflection;

public static class TypeHelper
{
    public static object CreateInstanceByClassName(string className, object[] ctorArgs)
    {
        // ���������Ѽ��س���
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // ������ className ��β�����ͣ���ֹ�����ռ䲻ͬ��
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == className);
            if (type != null)
            {
                return Activator.CreateInstance(type, ctorArgs);
            }
        }
        throw new Exception($"δ�ҵ�����: {className}");
    }
}