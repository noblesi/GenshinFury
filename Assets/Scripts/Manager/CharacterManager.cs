using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����, ���� �� ĳ������ �������� ����
public class CharacterManager : MonoSingleton<CharacterManager>
{
    public GameObject Player = null;

    // ��ü ���� ���� �����̳�
    public List<GameObject> Enemy = null;

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public GameObject GetEnemy()
    {
        if(Enemy.Count <= 0)
        {
            CreateEnemy();
        }

        return null;
    }

    public GameObject GetPlayer()
    {
        if(Player == null)
        {
            CreatePlayer();
        }
        return null;
    }
    public void SetEnemy(GameObject enemy)
    {
        Enemy.Add(enemy);
    }

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }

    private void CreateEnemy()
    {
        for(int i = 0; i < 4; i++)
        {
            Enemy.Add(new GameObject());
        }
    }

    private void CreatePlayer()
    {
        Player = new GameObject();  
    }

}
