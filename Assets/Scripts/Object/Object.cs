using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OBJECT_TYPE
{
    NONE,
    DOOR,
    BOX,
    ITEM
}

public class Object : MonoBehaviour
{
    public OBJECT_TYPE Object_Type = OBJECT_TYPE.NONE;

    public void Interaction()
    {
        Action();
    }
    protected void Action()
    {

    }

    protected void Timeout()
    {

    }
}
