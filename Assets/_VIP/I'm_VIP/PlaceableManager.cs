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
    public List<MyProjectile> MineProjectile = new List<MyProjectile>();
    public List<MyProjectile> OpponentProjectile = new List<MyProjectile>();
    public AIBase opponentTower , mineTower;

    void Awake()
    {
        Instance = this;
        Opponent.Add(opponentTower.GetComponent<PlaceableView>());
        Mine.Add(mineTower.GetComponent<PlaceableView>());
    }

    void Update()
    {
        UpdatePlaceable(Mine);
        UpdatePlaceable(Opponent);
        UpdateProjectiles(MineProjectile);
        UpdateProjectiles(OpponentProjectile);
    }

    private void UpdateProjectiles(List<MyProjectile> myProjectilesList)
    {
        List<MyProjectile> destroyProjectilesList = new List<MyProjectile>();   
        for (int i = 0; i < myProjectilesList.Count; i++)
        {
            var MP = myProjectilesList[i];
            var CasterAI = MP.caster as UnitAI;
            var TargetAI = MP.target;
            if(TargetAI == null)
            {
                Destroy(MP.gameObject);
                //利用 destroyProjectilesList 來儲存被摧毀的投擲物，如果直接Remove會導致List的Index往前推進下次搜尋時會跳過
                destroyProjectilesList.Add(MP);
                continue;   
            }
            MP.progress += Time.deltaTime * MP.projectileSpeed;
            MP.transform.position = Vector3.Lerp(MP.caster.transform.position, TargetAI.transform.position + Vector3.up, MP.progress);           

            if (MP.progress >= 1f)
            {                
                CasterAI.OnDealDamage();
                if (TargetAI.GetComponent<PlaceableView>().placeableData.hitPoints <= 0)
                {
                    if (TargetAI.aiState != AIState.Dead)
                    {
                        TargetAI.aiState = AIState.Dead;
                    }
                    TargetAI.GetComponent<Animator>()?.SetTrigger("IsDead");
                }
            }
            Destroy(MP.gameObject);
            destroyProjectilesList.Add(MP);
        }
        foreach (var item in destroyProjectilesList)
        {
            myProjectilesList.Remove(item);
        }
    }

    private void UpdatePlaceable(List<PlaceableView> placeableViewsList)
    {
        for (int i = 0; i < placeableViewsList.Count; i++)
        {            
            PlaceableView PV = placeableViewsList[i];
            MyPlaceable PData = PV.placeableData;
            AIBase AIB = PV.GetComponent<AIBase>();
            NavMeshAgent NMA = AIB.GetComponent<NavMeshAgent>();
            Animator Ani = AIB.GetComponent<Animator>();


            switch (AIB.aiState)
            {
                case AIState.Idle:
                    if (AIB is BuildingAI)
                    {
                        //TODO 如果塔要攻擊則設定為攻擊狀態
                        break;
                    }
                    //場景內最近的敵人
                    AIB.target = FindClosestEnemy(PData.faction, AIB.transform.position);

                    if (AIB.target != null)
                    {
                        Debug.Log($"找到最近的敵人{AIB.target.gameObject}");
                        AIB.aiState = AIState.Seek;
                        NMA.enabled = true;
                        Ani.SetBool("IsMoving", true);
                    }
                    else
                    {
                        Ani.SetBool("IsMoving", false);
                    }

                    //判斷是否有敵人在範圍內
                    //有則切換Seek狀態
                    break;
                case AIState.Seek:
                    //往敵人方向前進
                    if (AIB.target == null)
                    {
                        AIB.aiState = AIState.Idle;
                        break;
                    }
                    NMA.SetDestination(AIB.target.transform.position);

                    //判定是否進入攻擊範圍，若是則進入Attack狀態                    
                    if (IsInAttackRange(PV.transform.position, AIB.target.transform.position, PData.attackRange))
                    {
                        NMA.enabled = false;
                        AIB.aiState = AIState.Attack;
                    }
                    break;
                case AIState.Attack:

                    if (AIB.target == null)
                    {
                        AIB.aiState = AIState.Idle;
                        break;
                    }
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
                    //AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints -= PData.damagePerAttack;
                    if (AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints <= 0)
                    {
                        AIB.target.GetComponent<AIBase>().aiState = AIState.Dead;
                        AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints = 0;                        
                        AIB.target.GetComponent<Animator>()?.SetTrigger("IsDead");
                        AIB.aiState = AIState.Idle;
                    }
                    AIB.lastBlowTime = Time.time;
                    break;
                case AIState.Dead:
                    {
                        if (AIB is BuildingAI) 
                            break;                       
                        NMA.enabled = false;                        
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
            //如果小兵距離比較則紀錄跟下一個做比對
            if (dis < min && unit.GetComponent<AIBase>().aiState != AIState.Dead)
            {
                min = dis;
                closest = unit.GetComponent<AIBase>();
            }
        }
        return closest;
    }
}
