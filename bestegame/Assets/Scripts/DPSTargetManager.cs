using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPSTargetManager : MonoBehaviour
{
    public Text txt;
    float nextActionTime = 0f;
    float period = 1f;
    float dps = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < transform.childCount;i++)
        {
            dps += (transform.GetChild(i)).GetComponent<DPSTarget>().damage;
            (transform.GetChild(i)).GetComponent<DPSTarget>().damage = 0;
        }

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            txt.text = "Dps: " + Mathf.Round(dps).ToString();
            dps = 0;
        }
    }
}
