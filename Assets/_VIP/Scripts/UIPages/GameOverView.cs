using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityRoyale.Placeable;

public partial class GameOverPage
{
	public GameOverPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
		Debug.LogWarning("TODO: 请修改GameOverPage页面类型等参数，或注释此行");
	}

	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
		oKButton.onClick.AddListener(() =>
		{
			CloseAllPages();
			Addressables.LoadSceneAsync("Main");
		});
    }


    protected override void OnActive()
    {
        var faction = (Faction)data;
        var winner = faction == Faction.Player ? kingRed : kingBlue;

        var cg = winner.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 1.5f);

        winner.DOShakeScale(1.5f);
        winImage.transform.localPosition = winner.localPosition;
    }
    //public void MyEventHandler()
    //{
    //}
}
