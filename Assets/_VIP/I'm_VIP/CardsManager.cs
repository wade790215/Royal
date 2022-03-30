using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;
    //public GameObject[] cardPrefabs;
    public Transform[] cardsTrans = new Transform[3];
    public Transform startPos, endPos;
    public Transform canvas;
    private Transform previewCard;
    public Transform previewHodler;
    public MeshRenderer forbiddenAreaRenderer;
    
    private void Awake()
    {
        Instance = this;
    }
    async void Start()
    {        
        await (CreateOneCardToPreveiwArea(0.5f));
        await (PreveiwAreaToPlayingArea(0, 1f));

        await (CreateOneCardToPreveiwArea(1.5f));
        await (PreveiwAreaToPlayingArea(1, 2f));

        await (CreateOneCardToPreveiwArea(2.5f));
        await (PreveiwAreaToPlayingArea(2, 3f));

        await (CreateOneCardToPreveiwArea(3.5f));
    }
    //改用async來降低效能消耗
    public async Task CreateOneCardToPreveiwArea(float delayTime) 
    {        
        //yield return new WaitForSeconds(delayTime);

        //這裡會創建一個Task，返回Task類型
        await new WaitForSeconds(delayTime); 

        //Random.Range若為int則不會取得最大值
        int iCrad = Random.Range(0,MyCardModel.instance.list.Count);         
        var card = MyCardModel.instance.list[iCrad];

        //var cardGO = Resources.Load<GameObject>(card.cardPrefab);
        //GameObject cardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Length)];
        //previewCard  = Instantiate(cardGO, canvas).transform;

        // Async異步實例化，需等到實例化完成才能得到物件 InstantiateAsync =  Resources.Load + Instantiate
        GameObject cardPrefab =  await Addressables.InstantiateAsync(card.cardPrefab).Task;
        previewCard = cardPrefab.transform;
        previewCard.SetParent(canvas, false);
        previewCard.position = startPos.position;
        previewCard.localScale = Vector3.one ;
        previewCard.DOMove(endPos.position, 0.1f);
        previewCard.GetComponent<CardView>().data = card;   
        previewCard.GetComponent<CardView>().previewHolder = this.previewHodler;
    }   

    public async Task PreveiwAreaToPlayingArea(int playAreaIndex ,float delayTime)
    {
        await new WaitForSeconds(delayTime);
        previewCard.localScale = Vector3.one;
        previewCard.DOMove(cardsTrans[playAreaIndex].position, 0.5f);
        previewCard.GetComponent<CardView>().playAreaIndex = playAreaIndex;
    }
}
