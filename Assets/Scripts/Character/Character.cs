using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CHARACTER_TYPE
{
    NONE,
    PLAYER,
    ENEMY
}
public enum CHARACTER_STATE
{
    NONE,
    IDLE,
    MOVE,
    ATTACK,
    DEAD
}
public class Character : MonoBehaviour
{
    [System.Serializable]
    public class StatusClass
    {
        public int HP = 0;
        public int MP = 0;
        public int Attack = 0;
        public int Defense = 0;
        public float Range = 0f;
        public float Speed = 0f;

        /// <summary>
        /// 데미지를 받았을 때 처리하는 함수
        /// 반환값이 true면 사망, false이면 생존
        /// </summary>
        public bool SetDamage(int damageValue)
        {
            HP -= damageValue;

            return HP <= 0;
        }
    }

    public CHARACTER_TYPE Character_Type = CHARACTER_TYPE.NONE;
    public CHARACTER_STATE State = CHARACTER_STATE.NONE;
    public StatusClass Stat = null;

    public Character Target = null;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        State = CHARACTER_STATE.IDLE;

        Stat.Speed = 1f;
        Stat.Range = 1f;
        Stat.Attack = 1;
        Stat.HP = 1;
    }

    private void FixedUpdate()
    {
        switch (State)
        {
            case CHARACTER_STATE.NONE:
                break;
            case CHARACTER_STATE.IDLE:
                {
                    Idle();
                }
                break;
            case CHARACTER_STATE.MOVE:
                {
                    Move();
                }
                break;
            case CHARACTER_STATE.ATTACK:
                {
                    Attack();
                }
                break;
            case CHARACTER_STATE.DEAD:
                {
                    Die();
                }
                break;
            default:
                break;
        }
    }
    public void Update()
    {
        
    }

    public void SetState(CHARACTER_STATE state)
    {
        State = state;
    }

    public void Idle()
    {
        if (Target == null) Search();

        if (Target != null) SetState(CHARACTER_STATE.MOVE);
    }

    protected void Search()
    {
        Character target = null;
        switch (Character_Type)
        {
            case CHARACTER_TYPE.NONE:
                break;
            case CHARACTER_TYPE.PLAYER:
                target = GameManager.Instance.GetClosestEnemy(float.MaxValue);
                break;
            case CHARACTER_TYPE.ENEMY:
                target = GameManager.Instance.GetPlayer();
                break;
            default:
                break;
        }

        Target = target;

        if(Target != null)
        {
            Debug.Log(Target.gameObject.name);
        }
        
    }
    public void Move()
    {
        Vector3 dirVec = Target.transform.position - transform.position;

        float dist = dirVec.magnitude;

        if(dist > 1f) // 공격 사정거리
        {
            MovePos(dirVec.normalized);
        }
        else
        {
            SetState(CHARACTER_STATE.ATTACK);
        }

        MovePos(dirVec.normalized);
    }

    protected void MovePos(Vector3 dir)
    {
        transform.position += dir * Stat.Speed * Time.deltaTime;
    }

    public void Attack()
    {
        Target.SetDamage(Stat.Attack); // 공격력

        Debug.Log("Attack" + gameObject.name);
    }

    public void SetDamage(int damageValue)
    {
        Debug.Log("Damage" + gameObject.name);
        bool isDie = Stat.SetDamage(damageValue);

        if (isDie) Die();
    }

    public void Die()
    {
        Debug.Log("Die" + gameObject.name);
        gameObject.SetActive(false);
    }
}
