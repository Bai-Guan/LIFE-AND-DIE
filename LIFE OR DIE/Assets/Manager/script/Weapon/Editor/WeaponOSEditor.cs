using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(WeaponData))]
public class WeaponOSEditor : Editor
{
    private static List<Type> dataComptypes = new List<Type>();

    WeaponData DataSO;

    private void OnEnable()
    {
        DataSO =target as WeaponData;
    }
    //之后用户点击交互时候 会重新跑一遍这个方法
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        foreach (var type in dataComptypes)
        {
            //GUILayout是自动排列的GUI自定义类
            if(GUILayout.Button(type.Name))
            {
                //NET 反射方法的一种 可以将泛型实例化
                var comp = Activator.CreateInstance(type) as ComponentData;
                if(comp == null ) return; 
                DataSO.AddData(comp);
            }
        }

    }
    //让编译器每次编译后 编译界面调用此函数
    [DidReloadScripts]
     private static void OnRecompile()
     {

        //AppDomain（应用程序域） 是.NET框架中的一个概念，可以理解为应用程序中的"轻量级进程".
        //它提供了代码执行隔离、安全边界和程序集加载 / 卸载机制。
        //AppDomain.CurrentDomain 是一个静态属性，返回当前代码正在执行的应用程序域。在Unity中，这通常就是主应用程序域。
        //GetAssemblies() 返回当前应用程序域中已加载的所有程序集的数组。
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //在返回的所有程序集中 创建一个管道 IE遍历器
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        //Where 是 LINQ 的过滤方法，用于从集合中筛选满足条件的元素。
        //type.ContainsGenericParameters看是否包含泛型
        //通常类的where T:XXXX 是起到约束作用，比如A where T：B 就是A（某个子类）只能继承B（父类
        var filteredTypes = types.Where(
            type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters &&type.IsClass
            );
        //此处程序才正在开始运行 通过以上的管道
        dataComptypes=filteredTypes.ToList();
     }
 }
