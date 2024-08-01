using System;
using UnityEngine;

[Serializable]
public class Character
{
    public string name;  // name 필드 추가
    public int level;
    public SerializableVector3 position;
    public DateTime savedTime;

    public Character() { }

    public Character(string characterName, int level, DateTime savedTime)
    {
        name = characterName;  // 기본적으로 characterName을 name 필드에 할당
        this.level = level;
        this.savedTime = savedTime;
        position = new SerializableVector3(Vector3.zero);  // 기본 위치 설정
    }
}

[Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
