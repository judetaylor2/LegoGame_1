using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    bool isAttacking;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            Transform t = transform;
            t.LookAt(player.position);
            transform.rotation = Quaternion.Euler(0f, t.eulerAngles.y, 0f);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Harker_Attack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Harker_Attack2"))
            {
                anim.SetBool("attack1", false);
                anim.SetBool("attack2", false);
            }
            else
            {
                anim.Play(Random.Range(0,2) == 1 ? "Harker_Attack1" : "Harker_Attack2");
            }
        }
        else
        {
            anim.Play("Harker_Run");
            agent.SetDestination(player.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        isAttacking = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        isAttacking = false;
    }
}
