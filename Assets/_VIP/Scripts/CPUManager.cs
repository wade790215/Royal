using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityRoyale.Placeable;

public class CPUManager : MonoBehaviour
{
    private float PlayIntervalTime = 3.0f;
    public Transform[] cpuPlayCardRange = new Transform[2];
    private bool isGameOver;
    // Start is called before the first frame update
    async void Start()
    {
        KBEngine.Event.registerOut("OnGameOver", this, "OnGameOver");
        await CreateRandomCards();
    }

    public void OnGameOver(Faction faction)
    {
        isGameOver = true;
        Debug.Log($"GameOver{faction}");
    }

    async Task CreateRandomCards()
    {       
        while (true)
        {
            await new WaitForSeconds(PlayIntervalTime);
            var cardList = MyCardModel.instance.list;
            var carData = cardList[Random.Range(0, cardList.Count)];
            //Tips 每個線程都要加上isGameOver判斷，為什麼不加在While因為當程式執行到這行時會開出線程等待，有機會導致多出兵一次
            if (isGameOver) break;           
            var viewList = await CardView.CreatePlaceable(carData, new Vector3(Random.Range(cpuPlayCardRange[0].position.x, cpuPlayCardRange[1].position.x), 0, 
                Random.Range(cpuPlayCardRange[0].position.z, cpuPlayCardRange[1].position.z)), transform, Faction.Opponent);

            foreach (var item in viewList)
            {
                PlaceableManager.Instance.Opponent.Add(item);
            }
            if (isGameOver) break;
        }
    }   
}
