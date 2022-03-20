using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityRoyale.Placeable;

public class CPUManager : MonoBehaviour
{
    private float PlayIntervalTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateRandomCards());
    }

    IEnumerator CreateRandomCards()
    {
        while (true)
        {
            yield return new WaitForSeconds(PlayIntervalTime);
            var cardList = MyCardModel.instance.list;
            var carData = cardList[Random.Range(0, cardList.Count)];
            var viewList = CardView.CreatePlaceable(carData, new Vector3(Random.Range(-8f, 8f), 0, Random.Range(2, 8)), transform, Faction.Opponent);
            foreach (var item in viewList)
            {
                PlaceableManager.Instance.Opponent.Add(item);
            }            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
