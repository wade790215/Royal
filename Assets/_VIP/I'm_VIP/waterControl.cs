using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterControl : MonoBehaviour
{
    private Animator waterAni;
    private float flowValue = 0f;
    private float scateredValue = 0;
    private bool isSwtichWater = false; 

    // Start is called before the first frame update
    void Start()
    {
        waterAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if(flowValue >= 1f) 
                flowValue = 1f;
            flowValue += Time.deltaTime;              
            waterAni.SetFloat("Flow", flowValue);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (flowValue <= 0f)
                flowValue = 0f;
            flowValue -= Time.deltaTime;
            waterAni.SetFloat("Flow", flowValue);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (scateredValue >= 1f)
                scateredValue = 1f;
            scateredValue += Time.deltaTime;
            waterAni.SetFloat("Scattered", scateredValue);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (scateredValue <= 0f)
                scateredValue = 0f;
            scateredValue -= Time.deltaTime;
            waterAni.SetFloat("Scattered", scateredValue);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isSwtichWater)
            {
                waterAni.SetTrigger("SwitchWater");
                isSwtichWater = true;
            }             
        }
        if(isSwtichWater)
            isSwtichWater = false;
    }
}
