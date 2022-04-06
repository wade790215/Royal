using System.Collections.Generic;
using UnityEngine;

public partial class GameOverPage
{
	public GameOverPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
		Debug.LogWarning("TODO: 请修改GameOverPage页面类型等参数，或注释此行");
	}

	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
	}

	//public void MyEventHandler()
	//{
	//}
}
