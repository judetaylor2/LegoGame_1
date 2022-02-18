using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth, health, coins;
    public GameObject legoParticle, heartObject, coinObject, keyObject, loseScreen;
    public List<GameObject> playerHearts;
    bool isDestroyed;

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0 && !isDestroyed)
        {
            Instantiate(legoParticle, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Instantiate(heartObject, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            for (int i = 0; i < Random.Range(4, 8); i++)
            Instantiate(coinObject, transform.position + new Vector3(Random.Range(-5, 5), 0f, Random.Range(-5, 5)), Quaternion.Euler(-90f, 0f, 0f));

            if (keyObject != null)
            keyObject.transform.parent = null;

            if (gameObject.tag == "Player")
            {
                loseScreen.SetActive(true);
                StartCoroutine("ReloadScene");
                
                MeshRenderer[] l = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer m in l)
                {
                    m.enabled = false;
                }

                GetComponent<PlayerController>().enabled = false;
            }
            else
            {
                Destroy(gameObject);
            }

            isDestroyed = true;
            
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

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
