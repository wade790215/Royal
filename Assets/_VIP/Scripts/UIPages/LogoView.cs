using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public partial class LogoPage
{
    private float showSeconds = 4f;
    
    public void OnStart()
    {
        //KBEngine.Event.registerOut("MyEventName", this, "MyEventHandler");
        //progess.value = 0;
        //progess.DOValue(1.0f, showSeconds).OnComplete(() => ShowPageAsync<MainPage>());
    }

    protected override void OnActive()
    {
        ShowPageAsync<LoginPage>();
    }

    //public void MyEventHandler()
    //{
    //}
}
