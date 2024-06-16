using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �� ����
public class DungeonManager : MonoSingleton<DungeonManager>
{
    public List<GameObject> Dungeon = null;

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public GameObject GetDungeon()
    {
        if(Dungeon.Count <= 0)
        {
            CreateDungeon();
        }

        return null;
    }

    public void SetDungeon(GameObject dungeon)
    {
        Dungeon.Add(dungeon);
    }

    public void CreateDungeon()
    {
        Dungeon.Add(new GameObject());
    }
}
