using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;
    //public GameObject[] cardPrefabs;
    public Transform[] cardsTrans = new Transform[3];
    public Transform startPos, endPos;
    public Transform canvas;
    private Transform previewCard;
    public Transform previewHodler;
    
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {        
        StartCoroutine(CreateOneCardToPreveiwArea(0.5f));
        StartCoroutine(PreveiwAreaToPlayingArea(0, 1f));

        StartCoroutine(CreateOneCardToPreveiwArea(1.5f));
        StartCoroutine(PreveiwAreaToPlayingArea(1, 2f));

        StartCoroutine(CreateOneCardToPreveiwArea(2.5f));
        StartCoroutine(PreveiwAreaToPlayingArea(2, 3f));
    }

    public IEnumerator CreateOneCardToPreveiwArea(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        int iCrad = Random.Range(0,MyCardModel.instance.list.Count-1); //隨機選擇卡牌
        var card = MyCardModel.instance.list[iCrad];
        var cardGO = Resources.Load<GameObject>(card.cardPrefab);
        //ameObject cardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Length)];
        previewCard  = Instantiate(cardGO, canvas).transform;
        //previewCard.SetParent(canvas, false); //設置於父節點下(0,0,0) 
        previewCard.position = startPos.position;
        previewCard.localScale = Vector3.one ;
        previewCard.DOMove(endPos.position, 0.1f);
        previewCard.GetComponent<MyCardView>().data = card;   
        previewCard.GetComponent<MyCardView>().previewHolder = this.previewHodler;
    }   

    public IEnumerator PreveiwAreaToPlayingArea(int playAreaIndex ,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        previewCard.localScale = Vector3.one;
        previewCard.DOMove(cardsTrans[playAreaIndex].position, 0.5f);
        previewCard.GetComponent<MyCardView>().playAreaIndex = playAreaIndex;
    }
}
