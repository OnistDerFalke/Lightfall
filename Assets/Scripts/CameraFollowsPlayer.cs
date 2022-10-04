using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowsPlayer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    void Update()
    {
        var camPos = cam.transform.position;
        camPos.x = transform.position.x;
        camPos.y = transform.position.y;
        cam.transform.position = camPos;
    }
}
