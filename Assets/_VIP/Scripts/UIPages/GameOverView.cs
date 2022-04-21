using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityRoyale.Placeable;

public partial class GameOverPage
{
	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
		oKButton.onClick.AddListener(() =>
		{
			CloseAllPages();
			Addressables.LoadSceneAsync("MyMain");
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
