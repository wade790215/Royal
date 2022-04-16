using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public partial class MainPage
{	
	public void OnStart()
	{
		//KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
		
		battleButton.onClick.AddListener(() =>
		{
			Addressables.LoadSceneAsync("Game").Completed += GameSceneCompleted;
		});
	}

    private void GameSceneCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        CloseAllPages();
    }

    protected override void OnActive()
    {
		ShowPageAsync<TopFixPage>();
		ShowPageAsync<BottomFixPage>();
		base.OnActive();
    }

    //public void MyEventHandler()
    //{
    //}
}
