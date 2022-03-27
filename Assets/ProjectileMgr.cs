using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMgr : MonoBehaviour
{
    public static ProjectileMgr Instance;
    public List<MyProjectile> MineProjectile = new List<MyProjectile>();
    public List<MyProjectile> OpponentProjectile = new List<MyProjectile>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            if (TargetAI == null)
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
                    PlaceableManager.Instance.OnEnterDie(TargetAI);
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
}
