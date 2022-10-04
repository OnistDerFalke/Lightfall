using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private float lightSpeed;
    
    [SerializeField] private int lightLimesUp, lightLimesDown;
    void Update()
    {
       SetLightRotation();
    }

    private void SetLightRotation()
    {
        var rot = gameObject.transform.localEulerAngles;
        
        if (Input.GetKey(KeyCode.RightArrow) && rot.z <= 180)
            rot.z = 360-rot.z;
        if(Input.GetKey(KeyCode.LeftArrow) && rot.z >= 180)
            rot.z = 360-rot.z;
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (rot.z >= lightLimesUp + 180 && rot.z <= lightLimesDown + 180)
            {
                rot.z += lightSpeed;
                if (rot.z > lightLimesDown + 180) rot.z = lightLimesDown + 180;
            }
            else if(rot.z <= lightLimesDown && rot.z >= lightLimesUp)
            {
                rot.z -= lightSpeed;
                if (rot.z < lightLimesUp) rot.z = lightLimesUp;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (rot.z >= lightLimesUp + 180 && rot.z <= lightLimesDown + 180)
            {
                rot.z -= lightSpeed;
                if (rot.z < lightLimesUp + 180) rot.z = lightLimesUp + 180;
            }
            else if(rot.z <= lightLimesDown && rot.z >= lightLimesUp)
            {
                rot.z += lightSpeed;
                if (rot.z > lightLimesDown) rot.z = lightLimesDown;
            }
        }

        rot.z %= 360;
        Debug.Log(rot);
        gameObject.transform.localEulerAngles = rot;
    }
}
