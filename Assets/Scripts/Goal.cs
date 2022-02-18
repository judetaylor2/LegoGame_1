using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public GameObject winScreen;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            winScreen.SetActive(true);
            StartCoroutine("ReloadScene");
        }
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(5f);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        SceneManager.LoadScene(1);
        else
        SceneManager.LoadScene(0);
    }
}
