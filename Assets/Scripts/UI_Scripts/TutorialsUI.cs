using System;
using TMPro;
using UnityEngine;

public class TutorialsUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI altInteractButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    [SerializeField] private TextMeshProUGUI gamepadMoveButtonText;
    [SerializeField] private TextMeshProUGUI gamepadInteractButtonText;
    [SerializeField] private TextMeshProUGUI gamepadAltInteractButtonText;
    [SerializeField] private TextMeshProUGUI gamepadPauseButtonText;

    private void Start()
    {
        GameInput.Instance.OnInputRebind += GameInput_OnInputRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameCountDown())
        {
            Hide();
        }
    }



    private void GameInput_OnInputRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.MoveUp);
        moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.MoveDown);
        moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.MoveLeft);
        moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.MoveRight);
        interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact);
        altInteractButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.InteractAlternate);
        pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);
        gamepadInteractButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.GamepadInteract);
        gamepadAltInteractButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.GamepadInteractAlternate);
        gamepadPauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.GamepadPause);
        gamepadMoveButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.GamepadMove);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
