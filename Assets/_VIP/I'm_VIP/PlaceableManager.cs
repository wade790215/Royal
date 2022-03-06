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
            var PV = Mine[i];
            var PData = PV.placeableData;
            var AIB = PV.GetComponent<AIBase>();
            var NMA = AIB.GetComponent<NavMeshAgent>();
            var Ani = AIB.GetComponent<Animator>();

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
                    Ani.SetTrigger("Attack");
                    //執行攻擊動作
                    //檢測敵人是否仍然在攻擊範圍內或是生命值大於0
                    //若否則切換到Idle狀態
                    break;
                case AIState.Dead:
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
