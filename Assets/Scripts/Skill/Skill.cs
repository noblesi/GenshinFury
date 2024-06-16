using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_TYPE
{
    NONE,
    PROJECTILE,
    BOMB
}
public class Skill : MonoBehaviour
{
    public SKILL_TYPE Skill_Type = SKILL_TYPE.NONE;

    public void Call()
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
