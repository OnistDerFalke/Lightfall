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
        
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && rot.z <= 180)
            rot.z = 360-rot.z;
        if((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && rot.z >= 180)
            rot.z = 360-rot.z;
        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (rot.z >= lightLimesUp + 180 && rot.z <= lightLimesDown + 180)
            {
                rot.z += lightSpeed * Time.deltaTime;
                if (rot.z > lightLimesDown + 180) rot.z = lightLimesDown + 180;
            }
            else if(rot.z <= lightLimesDown && rot.z >= lightLimesUp)
            {
                rot.z -= lightSpeed * Time.deltaTime;
                if (rot.z < lightLimesUp) rot.z = lightLimesUp;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (rot.z >= lightLimesUp + 180 && rot.z <= lightLimesDown + 180)
            {
                rot.z -= lightSpeed * Time.deltaTime;
                if (rot.z < lightLimesUp + 180) rot.z = lightLimesUp + 180;
            }
            else if(rot.z <= lightLimesDown && rot.z >= lightLimesUp)
            {
                rot.z += lightSpeed * Time.deltaTime;
                if (rot.z > lightLimesDown) rot.z = lightLimesDown;
            }
        }

        rot.z %= 360;
        gameObject.transform.localEulerAngles = rot;
    }
}
