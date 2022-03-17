using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityRoyale.Placeable;

public partial class MyPlaceable
{
    public Faction faction = Faction.None;
    public MyPlaceable Clone()
    {
        return this.MemberwiseClone() as MyPlaceable;
    }
}

/// <summary>
/// 遊戲單位管理器
/// </summary>

public class PlaceableManager : MonoBehaviour
{
    public static PlaceableManager Instance;
    public List<PlaceableView> Mine = new List<PlaceableView>();
    public List<PlaceableView> Opponent = new List<PlaceableView>();
    public AIBase opponentTower;

    void Awake()
    {
        Instance = this;
        Opponent.Add(opponentTower.GetComponent<PlaceableView>());
    }


    void Update()
    {
        for (int i = 0; i < Mine.Count; i++)
        {
            //遊戲角色狀態
            PlaceableView PV = Mine[i];
            MyPlaceable PData = PV.placeableData;
            AIBase AIB = PV.GetComponent<AIBase>();
            NavMeshAgent NMA = AIB.GetComponent<NavMeshAgent>();
            Animator Ani = AIB.GetComponent<Animator>();
            
            switch (AIB.aiState)
            {
                case AIState.Idle:
                    //場景內最近的敵人
                    AIB.target = FindClosestEnemy(PData.faction, AIB.transform.position);

                    if (AIB.target != null)
                    {
                        Debug.Log($"找到最近的敵人{AIB.target.gameObject}");
                        AIB.aiState = AIState.Seek;
                        NMA.enabled = true;
                        Ani.SetBool("IsMoving", true);                        
                    }

                    //判斷是否有敵人在範圍內
                    //有則切換Seek狀態
                    break;
                case AIState.Seek:
                    //往敵人方向前進

                    NMA.SetDestination(AIB.target.transform.position);

                    //判定是否進入攻擊範圍，若是則進入Attack狀態                    
                    if (IsInAttackRange(PV.transform.position, AIB.target.transform.position, PData.attackRange))
                    {
                        NMA.enabled = false;
                        AIB.aiState = AIState.Attack;                       
                    }
                    
                    break;
                case AIState.Attack:
                    
                    if (IsInAttackRange(PV.transform.position, AIB.target.transform.position, PData.attackRange) == false)
                    {
                        AIB.aiState = AIState.Idle;
                        break;
                    }
                    //在攻擊間隔時間內不連續攻擊
                    if (Time.time < AIB.lastBlowTime + PData.attackRatio)
                    {
                        break;
                    }
                    Ani.SetTrigger("Attack");
                    AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints -= PData.damagePerAttack;
                    if (AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints <= 0)
                    {
                        AIB.aiState = AIState.Dead;
                        AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints = 0;
                        if(AIB.target.GetComponent<Animator>()!=null)
                            AIB.target.GetComponent<Animator>().SetTrigger("IsDead");
                    }
                    AIB.lastBlowTime = Time.time;
                    break;
                case AIState.Dead:
                    {
                        
                    }
                    break;
            }
        }
    }

    private bool IsInAttackRange(Vector3 myPos, Vector3 targetPos, float attackRange)
    {
        return Vector3.Distance(myPos, targetPos) < attackRange;
    }

    private AIBase FindClosestEnemy(Faction faction, Vector3 myPos)
    {
        var units = faction == Faction.Player ? Opponent : Mine;
        float min = float.MaxValue;
        AIBase closest = null;
        foreach (var unit in units)
        {
            var dis = Vector3.Distance(unit.transform.position, myPos);
            if (dis < min)
            {
                min = dis;
                closest = unit.GetComponent<AIBase>();
            }
        }
        return closest;
    }
}
