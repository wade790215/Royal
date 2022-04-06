using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class MainPage
{
	public MainPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		Debug.LogWarning("TODO: 请修改MainPage页面类型等参数，或注释此行");
	}

	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
		ShowPageAsync<TopFixPage>();
		ShowPageAsync<BottomFixPage>();
		battleButton.onClick.AddListener(() =>
		{
            CloseAllPages();
			Addressables.LoadSceneAsync("Game");		
		});
	}

	//public void MyEventHandler()
	//{
	//}
}
