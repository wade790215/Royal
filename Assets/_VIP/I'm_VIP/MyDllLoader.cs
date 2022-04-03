using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MyDllLoader : MonoBehaviour
{    
    //實現熱更新
    async void Start()
    {
        //Tips TextAsset可加載Text和二進制文件
        var dll =await Addressables.LoadAssetAsync<TextAsset>("HelloDll.dll").Task;
        var pdb =await Addressables.LoadAssetAsync<TextAsset>("HelloDll.pdb").Task;

        //加載到MONO虛擬機
        var ass = Assembly.Load(dll.bytes, pdb.bytes);
        //foreach (var item in ass.GetTypes())
        //{
        //    print(item);
        //}
        
        //取得Class
        var t = ass.GetType("Hello");
        //使用Class內的方法 Inovke(使用者,方法的參數)
        t.GetMethod("SayHello").Invoke(null,null);
    } 
}
