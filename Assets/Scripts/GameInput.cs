using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteraction;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerDefault.Enable();

        playerInputActions.PlayerDefault.Interact.performed += Interact_performed;
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
