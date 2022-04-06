using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class LogoPage
{
	private float showSeconds = 4f;
	//Normal 會隨著頁面而隱藏 Fixed 固定頁面，永遠顯示在最上層 PopUp 彈出頁面 Any 可關閉所有頁面
	public LogoPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		Debug.LogWarning("TODO: 请修改LogoPage页面类型等参数，或注释此行");
	}

	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
		progess.DOValue(1.0f, showSeconds).OnComplete(()=> ShowPageAsync<MainPage>());
	}

	//public void MyEventHandler()
	//{
	//}
}
