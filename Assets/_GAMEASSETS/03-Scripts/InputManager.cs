using LittleSubmarine2.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class InputManager : MonoBehaviour
    {
        public void BTN_Back(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (SceneManager.GetActiveScene().ToString().StartsWith("Level"))
                {
                    Message.Raise(new PauseGameEvent());
                }
                else if(SceneManager.GetActiveScene().ToString().Equals(Scenes.MAINMENU))
                {
                    Application.Quit();
                }
                else
                {
                    Message.Raise(new ToLastSceneEvent());
                }
            }
        }
    }
}