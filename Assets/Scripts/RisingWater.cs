using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float speed;
    public GameObject winScreen;

    // Update is called once per frame
    void Update()
    {
        if (!winScreen.activeInHierarchy)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
