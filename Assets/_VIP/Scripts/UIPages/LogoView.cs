using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class LogoPage
{
	private float showSeconds = 4f;
	public LogoPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
		Debug.LogWarning("TODO: 请修改LogoPage页面类型等参数，或注释此行");
	}

	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
		progess.DOValue(1.0f, showSeconds);
	}

	//public void MyEventHandler()
	//{
	//}
}
