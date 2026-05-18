using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteraction;
    public event EventHandler OnAltInteraction;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerDefault.Enable();

        playerInputActions.PlayerDefault.Interact.performed += Interact_performed;
        playerInputActions.PlayerDefault.InteractAlternate.performed += AltInteract_performed;
    }

    private void AltInteract_performed(InputAction.CallbackContext context)
    {
        OnAltInteraction?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteraction?.Invoke(this,EventArgs.Empty);
    }


    public Vector2 GetInputVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.PlayerDefault.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }


    
}
