using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventArgs
{
GameObject CollidedObject { get; }
    public Vector2 ContactPoint { get; }

    public PlayerEventArgs(GameObject Object, Vector2 Point)
    {
        CollidedObject = Object;
        ContactPoint = Point;
    }
}
