using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Particle_Move : MonoBehaviour
{
    public Transform targetTransform;
    public float duration = 5;
    //public float distance = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        // find the target transform
        //targetTransform = GameObject.Find("ParTarget").transform;
        transform.DOMove(targetTransform.position, duration)
            .SetLoops(-1, LoopType.Restart);
    }
    
}
