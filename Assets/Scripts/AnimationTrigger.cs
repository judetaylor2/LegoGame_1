using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        anim.SetBool("TriggerAnim", true);
    }
}
