using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_INPUT_MAPPING = "InputBindings";
    public static GameInput Instance {get ; private set;}
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteraction;
    public event EventHandler OnAltInteraction;
    public event EventHandler OnPauseGame;

    public enum Bindings
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        GamepadInteract,
        GamepadInteractAlternate,
        GamepadPause,
    }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_INPUT_MAPPING))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_INPUT_MAPPING));
        }

        playerInputActions.PlayerDefault.Enable();

        playerInputActions.PlayerDefault.Interact.performed += Interact_performed;
        playerInputActions.PlayerDefault.InteractAlternate.performed += AltInteract_performed;
        playerInputActions.PlayerDefault.Pause.performed += Pause_performed;
    }

    


    private void OnDestroy()
    {
        playerInputActions.PlayerDefault.Interact.performed -= Interact_performed;
        playerInputActions.PlayerDefault.InteractAlternate.performed -= AltInteract_performed;
        playerInputActions.PlayerDefault.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseGame?.Invoke(this, EventArgs.Empty);
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

    public string GetBindingText(Bindings binding)
    {
        switch (binding)
        {
            case Bindings.Interact:
            return playerInputActions.PlayerDefault.Interact.bindings[0].ToDisplayString();

            case Bindings.InteractAlternate:
            return playerInputActions.PlayerDefault.InteractAlternate.bindings[0].ToDisplayString();

            case Bindings.Pause:
            return playerInputActions.PlayerDefault.Pause.bindings[0].ToDisplayString();

            case Bindings.MoveUp:
            return playerInputActions.PlayerDefault.Move.bindings[1].ToDisplayString();

            case Bindings.MoveDown:
            return playerInputActions.PlayerDefault.Move.bindings[2].ToDisplayString();

            case Bindings.MoveLeft:
            return playerInputActions.PlayerDefault.Move.bindings[3].ToDisplayString();

            case Bindings.MoveRight:
            return playerInputActions.PlayerDefault.Move.bindings[4].ToDisplayString();

            case Bindings.GamepadInteract:
            return playerInputActions.PlayerDefault.Interact.bindings[1].ToDisplayString();

            case Bindings.GamepadInteractAlternate:
            return playerInputActions.PlayerDefault.InteractAlternate.bindings[1].ToDisplayString();

            case Bindings.GamepadPause:
            return playerInputActions.PlayerDefault.Pause.bindings[1].ToDisplayString();

            default:
            return "Unkown Binding";
        }
    }

    public void RebindBindings(Bindings binding, Action OnActionRebound)
    {
        playerInputActions.PlayerDefault.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Bindings.MoveUp:
                inputAction = playerInputActions.PlayerDefault.Move;
                bindingIndex = 1;
                break;
            
            case Bindings.MoveDown:
                inputAction = playerInputActions.PlayerDefault.Move;
                bindingIndex = 2;
                break;
            
            case Bindings.MoveLeft:
                inputAction = playerInputActions.PlayerDefault.Move;
                bindingIndex = 3;
                break;

            case Bindings.MoveRight:
                inputAction = playerInputActions.PlayerDefault.Move;
                bindingIndex = 4;
                break;

            case Bindings.Interact:
                inputAction = playerInputActions.PlayerDefault.Interact;
                bindingIndex = 0;
                break;
            
            case Bindings.InteractAlternate:
                inputAction = playerInputActions.PlayerDefault.InteractAlternate;
                bindingIndex = 0;
                break;
            
            case Bindings.Pause:
                inputAction = playerInputActions.PlayerDefault.Pause;
                bindingIndex = 0;
                break;

            case Bindings.GamepadInteract:
                inputAction = playerInputActions.PlayerDefault.Interact;
                bindingIndex = 1;
                break;
            
            case Bindings.GamepadInteractAlternate:
                inputAction = playerInputActions.PlayerDefault.InteractAlternate;
                bindingIndex = 1;
                break;
            
            case Bindings.GamepadPause:
                inputAction = playerInputActions.PlayerDefault.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.PlayerDefault.Enable();
                OnActionRebound();

                PlayerPrefs.SetString(PLAYER_INPUT_MAPPING, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }
    
}
