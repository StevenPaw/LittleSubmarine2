using System;
using System.Collections;
using System.Collections.Generic;
using LittleSubmarine2;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private bool activated;
    [SerializeField] private PushableTypes buttonType;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IPushable pushable = other.GetComponent<IPushable>();
        if (pushable != null)
        {
            if (pushable.GetPushableType() == buttonType)
            {
                activated = true;
            }
        }
        animator.SetBool("activated", activated);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        activated = false;
        animator.SetBool("activated", activated);
    }
}
