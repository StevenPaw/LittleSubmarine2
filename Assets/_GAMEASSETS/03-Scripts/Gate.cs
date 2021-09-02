using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleSubmarine2
{
    public class Gate : MonoBehaviour, IActivatable
    {
        [SerializeField] private bool activated;
        [SerializeField] private List<GameObject> neededActivatorsList = new List<GameObject>();

        private Collider2D gateCollider;
        private Animator gateAnimator;
        private List<IActivator> neededActivators = new List<IActivator>();

        private void Start()
        {
            gateCollider = GetComponent<Collider2D>();
            gateAnimator = GetComponent<Animator>();
            
            List<GameObject> wrongObjects = new List<GameObject>();
            
            foreach (GameObject activatorGO in neededActivatorsList)
            {
                if (activatorGO != null)
                {
                    IActivator activator = activatorGO.GetComponent<IActivator>();

                    if (activator != null)
                    {
                        neededActivators.Add(activator);
                        activator.AddActivatable(this);
                    }
                    else
                    {
                        wrongObjects.Add(activatorGO);
                    }
                }
            }

            foreach (GameObject wrongGO in wrongObjects)
            {
                neededActivatorsList.Remove(wrongGO);
            }
        }

        private void OnValidate()
        {
            List<GameObject> wrongObjects = new List<GameObject>();
            
            foreach (GameObject activatorGO in neededActivatorsList)
            {
                if (activatorGO != null)
                {
                    IActivator activator = activatorGO.GetComponent<IActivator>();

                    if (activator != null)
                    {
                    }
                    else
                    {
                        wrongObjects.Add(activatorGO);
                    }
                }
            }

            foreach (GameObject wrongGO in wrongObjects)
            {
                neededActivatorsList.Remove(wrongGO);
            }
        }

        public void CheckActivators()
        {
            bool everythingActive = true;
            foreach (IActivator activator in neededActivators)
            {
                if (!activator.GetActivated())
                {
                    everythingActive = false;
                    break;
                }
            }

            if (everythingActive)
            {
                activated = true;
            }
            else
            {
                activated = false;
            }
            
            gateAnimator.SetBool("activated", activated);
            gateCollider.enabled = !activated;
        }
    }
}