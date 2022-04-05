using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private bool canMove = true;
        private static PlayerInputManager instance; 
        private Vector2 rawAxis;
        
        public Vector2 RawAxis
        {
            get => rawAxis;
            set => rawAxis = value;
        }
        
        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }

        public static PlayerInputManager Instance => instance;

        void Start()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void BTN_Move(InputAction.CallbackContext ctx)
        {
            if (canMove)
            {
                rawAxis = ctx.ReadValue<Vector2>();
            }
        }
        
        public void BTN_BackToMenu()
        {
            SceneManager.LoadScene(Scenes.LEVELOVERVIEW);
        }

        public void OnEscape(InputAction.CallbackContext ctx)
        {
            BTN_BackToMenu();
        }
    }
}