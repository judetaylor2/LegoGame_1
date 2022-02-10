using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth, health, coins;
    public GameObject legoParticle, heartObject, coinObject;
    public List<GameObject> playerHearts;

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0 && gameObject.tag != "Player")
        {
            Instantiate(legoParticle, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Instantiate(heartObject, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            for (int i = 0; i < Random.Range(4, 8); i++)
            Instantiate(coinObject, transform.position + new Vector3(Random.Range(-5, 5), 0f, Random.Range(-5, 5)), Quaternion.Euler(-90f, 0f, 0f));
            
            Destroy(gameObject);
        }

        if (gameObject.tag == "Player")
        {
            bool[] isHeartVisible = new bool[maxHealth];
            for (int i = 0; i < health; i++)
            isHeartVisible[i] = true;

            for (int i = 0; i < maxHealth; i++)
            if (isHeartVisible[i])
            playerHearts[i].SetActive(true);
            else
            playerHearts[i].SetActive(false);

        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Player" && other.tag == "Heart" && health < 4 && health > 0)
        {
            health++;
            Destroy(other.gameObject);
        }
        else if (gameObject.tag == "Player" && other.tag == "Stud")
        {
            coins++;
            Destroy(other.gameObject);
        }

    }

}
