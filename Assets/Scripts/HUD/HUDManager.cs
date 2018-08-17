using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

    Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
        EventManager.StartListening("HitPlayer", HitPlayer);
	}
	
    void HitPlayer()
    {
        animator.Play("HitPlayer",-1,0f);
    }
}
