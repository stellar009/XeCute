using UnityEngine;
using UnityEngine.InputSystem;
using XeCute.Core;

namespace XeCute.Utils
{
    /// <summary>
    /// Global keyboard input. Listens for Space (Draw one card), P (Pause), R (Reset).
    /// v2: Space draws ONE card per press (not 5 at once).
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private RoundManager roundManager;
        [SerializeField] private UI.ControlsBar controlsBar;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (roundManager != null && roundManager.CanDraw)
                {
                    roundManager.DrawCard();
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (controlsBar != null) controlsBar.OnResetClicked();
            }
        }
    }
}
