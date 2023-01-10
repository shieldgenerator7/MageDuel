using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        Vector3 euler = transform.eulerAngles;
        euler.z += rotateSpeed * Time.deltaTime;
        euler.z %= 360;
        transform.eulerAngles = euler;
    }
}
