using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public static class CopyDll
{
    public static string source = "Library/ScriptAssemblies";
    public static string dest = "Assets/_VIP/I'm_VIP/Resources_moved/MyDll";
    public static string[] files = new string[] { "HelloDll.dll", "HelloDll.pdb" };

    [MenuItem("Tools/Copy Dll")]
    public static void CopyDll2MyDll()
    {
        //創建目錄
        Directory.CreateDirectory(dest);

        foreach (var item in files)
        {
            //如果副檔名是.dll Unity不能熱更新(不能打包) 所以才把副檔名加上.bytes
            Debug.Log($"{Path.Combine(source, item)} => {Path.Combine(dest, item + ".bytes")}");
            File.Copy(Path.Combine(source, item), Path.Combine(dest, item + ".bytes"), true);
        }

        AssetDatabase.Refresh();
    }
}
