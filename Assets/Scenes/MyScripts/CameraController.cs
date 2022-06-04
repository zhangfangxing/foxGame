using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    
    void Update()
    {
        // 每一帧都根据角色的x轴/y轴进行变化
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y,transform.position.z);
    }
}
