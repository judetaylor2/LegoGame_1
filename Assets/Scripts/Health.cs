using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth, health, coins;
    public GameObject legoParticle;

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0 && gameObject.tag != "Player")
        {
            Instantiate(legoParticle, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Destroy(gameObject);
        }
    }
}
