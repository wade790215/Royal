using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.Events;

#if UNITY_EDITOR
public class TestEditor : MonoBehaviour
{
    public UnityEvent[] unityEvents = new UnityEvent[3];



    [ContextMenu("AddIntListener")]
    void Start()
    {        
        for (int i = 0; i < unityEvents.Length; i++)
        {            
            UnityEventTools.AddIntPersistentListener(unityEvents[i], PrintTest, i);
        }      
    }
    [ContextMenu("RemoveAllListener")]
    private void RemoveListener()
    {
        for (int i = 0; i < unityEvents.Length; i++)
        {
            UnityEventTools.RemovePersistentListener<int>(unityEvents[i], PrintTest);
        }
    }
    public void PrintTest(int index)
    {
        Debug.Log(index);
    } 
    [MenuItem("MenuItem/SelectedObejct")]
    private static void SelectedObejct()
    {
        Debug.Log("Selected Success");
    }
    [MenuItem("MenuItem/SelectedObejct",true)]
    private static bool CheckObjectType()
    {
        Object selectObj = Selection.activeObject;
        if (selectObj != null && selectObj.GetType() == typeof(GameObject))
        {
            return true;
        }
       return false;    
    }

    [MenuItem("MyMenu/Do Something")]
    static void DoSomething()
    {
        Debug.Log("Doing Something...");
    }
#endif
}