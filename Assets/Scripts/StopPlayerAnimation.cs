using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayerAnimation : MonoBehaviour
{
    Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void stopAnim()
    {
        anim.SetBool("attack1", false);
        anim.SetBool("attack2", false);
    }
}
