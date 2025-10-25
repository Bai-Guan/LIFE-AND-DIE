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
    //֮���û��������ʱ�� ��������һ���������
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        foreach (var type in dataComptypes)
        {
            //GUILayout���Զ����е�GUI�Զ�����
            if(GUILayout.Button(type.Name))
            {
                //NET ���䷽����һ�� ���Խ�����ʵ����
                var comp = Activator.CreateInstance(type) as ComponentData;
                if(comp == null ) return; 
                DataSO.AddData(comp);
            }
        }

    }
    //�ñ�����ÿ�α���� ���������ô˺���
    [DidReloadScripts]
     private static void OnRecompile()
     {

        //AppDomain��Ӧ�ó����� ��.NET����е�һ������������ΪӦ�ó����е�"����������".
        //���ṩ�˴���ִ�и��롢��ȫ�߽�ͳ��򼯼��� / ж�ػ��ơ�
        //AppDomain.CurrentDomain ��һ����̬���ԣ����ص�ǰ��������ִ�е�Ӧ�ó�������Unity�У���ͨ��������Ӧ�ó�����
        //GetAssemblies() ���ص�ǰӦ�ó��������Ѽ��ص����г��򼯵����顣
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //�ڷ��ص����г����� ����һ���ܵ� IE������
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        //Where �� LINQ �Ĺ��˷��������ڴӼ�����ɸѡ����������Ԫ�ء�
        //type.ContainsGenericParameters���Ƿ��������
        //ͨ�����where T:XXXX ����Լ�����ã�����A where T��B ����A��ĳ�����ֻࣩ�ܼ̳�B������
        var filteredTypes = types.Where(
            type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters &&type.IsClass
            );
        //�˴���������ڿ�ʼ���� ͨ�����ϵĹܵ�
        dataComptypes=filteredTypes.ToList();
     }
 }
