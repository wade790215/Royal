using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using XCharts;

public class Test : MonoBehaviour
{
    List<VideoData> videoDatas = new List<VideoData>();
    Dictionary<int, GameObject> BundleDic = new Dictionary<int, GameObject>();
    List<Action<int>> closeUI = new List<Action<int>>();
    float time = 0f;
    public int currentIndex;
    public GameObject[] bundle;
    public GameObject go;
    public LineChart lineChart;
    void Start()
    {        
        //videoDatas = GetData<VR360Data>();
        //videoDatas = videoDatas.OrderBy((t) => t.eventTime).ToList();
        //for (int i = 0; i < videoDatas.Count; i++)
        //{
        //      Instantiate(AssetManager.Load<GameObject>(videoDatas.AssetID));
        //    //BundleDic.add(videoDatas.eventTime, AssetManager.Load<GameObject>(videoDatas.AssetID));
        //}        
        for (int i = 0; i < 5; i++)
        {
            VideoData VD = new VideoData() { eventTime = i*3 , durationTime = i};
            videoDatas.Add(VD);
            closeUI.Add(CloseUI);
            BundleDic.Add(videoDatas[i].eventTime, bundle[i]);
        }        
        StartCoroutine(VideoEvent());
        lineChart.SetSize(600, 300);        
    }  

    private IEnumerator VideoEvent()
    {
        while (currentIndex <= videoDatas.Count-1)
        {            
            if (GetVideoTime() >= videoDatas[currentIndex].eventTime)
            {                
                if (BundleDic.TryGetValue(videoDatas[currentIndex].eventTime, out GameObject gameObject))
                {
                    go = gameObject;
                    go.transform.parent = this.transform;
                    go.SetActive(true);
                    closeUI[currentIndex]?.Invoke(videoDatas[currentIndex].durationTime);
                    //Invoke("CloseUI", videoDatas[currentIndex].durationTime);
                }
                if (currentIndex < videoDatas.Count - 1) currentIndex++;               
            }            
            yield return null;
        }        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lineChart.AnimationPause();            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            lineChart.AnimationResume();
        }
        //VideoEventManager(videoDatas); 
        FocusTarget(go);
    }

    void CloseUI(int duration)
    {
        go.SetActive(false);       
    }

    private float GetVideoTime()
    {
        return time += Time.deltaTime;
    }

    void VideoEventManager(List<VideoData> videoDatas)
    {
        for (int currentEventIndex = 0; currentEventIndex < videoDatas.Count; currentEventIndex++)
        {
            //if (VideoTime()>= videoDatas[currentEventIndex].eventTime && VideoTime() <= videoDatas[currentEventIndex].duration)
            //{
            if (BundleDic.TryGetValue(videoDatas[currentEventIndex].eventTime, out GameObject gameObject))
            {
                gameObject.transform.parent = null;
                gameObject.SetActive(true);
                FocusTarget(gameObject);
            }
            //}
        }
    }

    private void FocusTarget(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
    }
}

internal class VideoData
{
    internal int eventTime;
    internal int durationTime;   
}