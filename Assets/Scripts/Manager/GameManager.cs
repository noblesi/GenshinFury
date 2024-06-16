using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    // ������ ���� ����.

    private int dungeon_level = 0;

    public Character Player = null;
    public List<Character> Enemy = null;

    public void Init()
    {

    }

    public void Clear()
    {

    }

    private void FixedUpdate()
    {
        
    }

    public void DungeonSetting()
    {

    }

    public void DungeonStart()
    {

    }

    public void DungeonEnd()
    {

    }

    public GameObject GetEnemy()
    {
        return null;
    }
    /// <summary>
    /// ���� ����� ���� ��� ����
    /// </summary>
    /// <param name="dist">Ž�� ���� ��</param>
    /// <returns></returns>
    public Character GetClosestEnemy(float dist)
    {
        Character closestEnemy = null;

        float tmpDist = 0f;
        var playerPosition = Player.transform.position;
        Enemy.ForEach(enemy =>
        {
            tmpDist = Vector3.Distance(playerPosition, enemy.transform.position);
            if(dist > tmpDist)
            {
                dist = tmpDist;
                closestEnemy = enemy;
            }
        });

        return closestEnemy;
    }

    public Character GetPlayer()
    {
        return Player;
    }

    public void EnemyDie()
    {

    }

    public void PlayerDie()
    {

    }
}
