using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GruntBaseState 
{
    protected GruntController controller;
   public GruntBaseState(GruntController controller)
    {
        this.controller = controller;   
    }
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
