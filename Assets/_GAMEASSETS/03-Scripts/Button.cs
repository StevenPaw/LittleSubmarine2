using System;
using System.Collections;
using System.Collections.Generic;
using LittleSubmarine2;
using UnityEngine;

namespace LittleSubmarine2
{
    public class Button : MonoBehaviour, IActivator
    {
        [SerializeField] private bool activated;
        [SerializeField] private PushableTypes buttonType;

        private Animator animator;
        private List<IActivatable> activatables = new List<IActivatable>();

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
            CheckActivatables();
            Debug.Log("Entered Button!");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IPushable pushable = other.GetComponent<IPushable>();
            if (pushable != null)
            {
                if (pushable.GetPushableType() == buttonType)
                {
                    activated = false;
                }
            }

            animator.SetBool("activated", activated);
            CheckActivatables();
        }

        private void CheckActivatables()
        {
            foreach (IActivatable activatable in activatables)
            {
                activatable.CheckActivators();
            }
        }

        public bool GetActivated()
        {
            return activated;
        }

        public void Activate()
        {
            activated = true;
        }

        public void DeActivate()
        {
            activated = false;
        }

        public void AddActivatable(IActivatable activatableIn)
        {
            activatables.Add(activatableIn);
        }
    }
}