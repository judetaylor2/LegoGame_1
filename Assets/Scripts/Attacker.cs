using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    Health h;

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Health>(out h))
        h.health--;    
    }
}
