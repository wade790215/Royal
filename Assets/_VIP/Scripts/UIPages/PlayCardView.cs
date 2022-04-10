using System.Collections.Generic;
using UnityEngine;

public partial class PlayCardPage
{
	public PlayCardPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
		Debug.LogWarning("TODO: 请修改PlayCardPage页面类型等参数，或注释此行");
	}

	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
		
	}

    protected override void OnActive()
    {
		base.OnActive();
		CardsManager.Instance.startPos = startPos;
		CardsManager.Instance.endPos = endPos;
		CardsManager.Instance.canvas = transform;
		for (int i = 0; i < cardTrans.childCount; i++)
		{
			CardsManager.Instance.cardsTrans[i] = cardTrans.GetChild(i);
		}
	}

    //public void MyEventHandler()
    //{
    //}
}
