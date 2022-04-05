using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class PlayerOnScreenController : MonoBehaviour
    {
        private PlayerInputManager playerInputManager;

        private void Start()
        {
            playerInputManager = PlayerInputManager.Instance;
            playerInputManager.CanMove = true;
        }

        public void BTN_DirectionUpHold()
        {
            if (playerInputManager.CanMove)
            {
                playerInputManager.RawAxis = new Vector2(0, 1);
            }
        }
        
        public void BTN_DirectionDownHold()
        {
            if (playerInputManager.CanMove)
            {
                playerInputManager.RawAxis = new Vector2(0, -1);
            }
        }
        
        public void BTN_DirectionLeftHold()
        {
            if (playerInputManager.CanMove)
            {
                playerInputManager.RawAxis = new Vector2(-1, 0);
            }
        }
        
        public void BTN_DirectionRightHold()
        {
            if (playerInputManager.CanMove)
            {
                playerInputManager.RawAxis = new Vector2(1, 0);
            }
        }

        public void BTN_DirectionRelease()
        {
            playerInputManager.RawAxis = Vector2.zero;
        }
        
        public void BTN_BackToMenu()
        {
            SceneManager.LoadScene(Scenes.LEVELOVERVIEW);
        }
    }
}