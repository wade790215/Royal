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
        progess.DOValue(1.0f, showSeconds).OnComplete(() => Addressables.LoadSceneAsync("Main"));
    }

    //public void MyEventHandler()
    //{
    //}
}
