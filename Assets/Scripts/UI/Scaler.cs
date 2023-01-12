using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public float scale = 2;
    public float duration = 1;

    private float startScale = 1;
    private float lastStartTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale.x;
        lastStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * Mathf.Lerp(
            startScale,
            scale,
            (Time.time - lastStartTime) / duration
            );
        if (Time.time > lastStartTime + duration)
        {
            lastStartTime = Time.time;
        }
    }
}
