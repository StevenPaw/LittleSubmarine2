using System;
using UnityEngine;

namespace LittleSubmarine2
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] private TutorialManager tutorialManager;
        [SerializeField] private int switchToStep;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTags.PLAYER))
            {
                tutorialManager.AdvanceTutorialSteps(switchToStep);
            }
        }
    }
}