using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityRoyale.Placeable;

public class CPUManager : MonoBehaviour
{
    private float PlayIntervalTime = 3.0f;
    public Transform[] cpuPlayCardRange = new Transform[2];
    // Start is called before the first frame update
    async void Start()
    {
        await CreateRandomCards();
    }

    async Task CreateRandomCards()
    {       
        while (true)
        {
            //間隔時間出牌
            await new WaitForSeconds(PlayIntervalTime);
            var cardList = MyCardModel.instance.list;
            var carData = cardList[Random.Range(0, cardList.Count)];
            //設定CPU出牌區域
            var viewList = await CardView.CreatePlaceable(carData, new Vector3(Random.Range(cpuPlayCardRange[0].position.x, cpuPlayCardRange[1].position.x), 0, 
                Random.Range(cpuPlayCardRange[0].position.z, cpuPlayCardRange[1].position.z)), transform, Faction.Opponent);
            foreach (var item in viewList)
            {
                PlaceableManager.Instance.Opponent.Add(item);
            }            
        }
    }   
}
