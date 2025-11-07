using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StackInteraction
{
    private static StackInteraction _instance;

    public static StackInteraction Instance
    {
      get { 
        if(_instance==null)
                _instance = new StackInteraction();
    
        return _instance;
        }
    }
    private readonly object _lock = new object();

    private Stack<GameObject> InterItem = new Stack<GameObject>();
    public int CurrentNum
    {
        get
        {
            return InterItem.Count;
        }
    }
    //通知各个组件 有物体被压入栈顶
    public event Action<GameObject> ActionPush;

    //这是一个事件，通知各位弹出了某个物品,以便刷新，并返回弹出后的数据个数,以及当前的栈顶
    public event Action<int,GameObject,bool> ActionPop;

 

    public void Push(GameObject item)
    {
        lock (_lock)
        {
            InterItem.Push(item);
            ActionPush?.Invoke(item);
        }
    }

    public GameObject PopSomeOne(GameObject item)
    {
        lock (_lock)
        {
            bool isFindDeleteObject = false;
            GameObject t=null;

            if( InterItem.Count <= 0 ) return null;
            bool isTop= InterItem.Peek()==item;
            var temp = new Stack<GameObject>(InterItem);
            InterItem.Clear();
            foreach (GameObject obj in temp)
            {
                if (obj != item)
                {
                    InterItem.Push(obj);
                }
                else
                {
                    t= obj;
                    isFindDeleteObject = true;
                }
            }

            ActionPop?.Invoke(InterItem.Count, InterItem.Count > 0 ? InterItem.Peek() : null,isTop);

            if (isFindDeleteObject)
                return t;
            else return null;
        }
    }

    public GameObject Peek()
    {
        GameObject temp = InterItem.Count > 0 ? InterItem.Peek() : null;
        return temp;
    }

    //public GameObject Pop()
    //{
    //    InterItem
    //}
}
