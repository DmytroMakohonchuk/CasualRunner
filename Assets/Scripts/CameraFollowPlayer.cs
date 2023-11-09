using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float cameraDodgeSpeed = 1f;
    private float x;

    private void Start()
    {
        x = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        var tempCameraPos = transform.position;
        var tempPlayerPos = player.transform.position;
        x = x * (1 - Time.deltaTime * cameraDodgeSpeed) + tempPlayerPos.x * (Time.deltaTime * cameraDodgeSpeed);
        transform.position = new Vector3(x, tempCameraPos.y, tempCameraPos.z);
    }
}
