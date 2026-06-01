using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button closeButton;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button altInteractButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadAltInteractButton;
    [SerializeField] private Button gamepadPauseButton;

    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI altInteractButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private TextMeshProUGUI gamepadInteractButtonText;
    [SerializeField] private TextMeshProUGUI gamepadAltInteractButtonText;
    [SerializeField] private TextMeshProUGUI gamepadPauseButtonText;

    [SerializeField] private Transform pressToRebindUI;

    private Action OnCloseAction;

    public static OptionsUI Instance {get; private set;}

    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(()=>
        {
            SoundEffectsManager.Instance.ChangeVolume();
            UpdateVisuals();
        });

        musicButton.onClick.AddListener(()=>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisuals();
        });

        closeButton.onClick.AddListener(()=>
        {
            Hide();
            OnCloseAction();
        });

        moveUpButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.MoveUp);
        });
        moveDownButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.MoveDown);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.MoveLeft);
        });
        moveRightButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.MoveRight);
        });
        interactButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.Interact);
        });
        altInteractButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.InteractAlternate);
        });
        pauseButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.Pause);
        });
        gamepadInteractButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.GamepadInteract);
        });
        gamepadAltInteractButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.GamepadInteractAlternate);
        });
        gamepadPauseButton.onClick.AddListener(() =>
        {
           RebindBindings(GameInput.Bindings.GamepadPause);
        });
        
    }
    private void Start()
    {
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;
        UpdateVisuals();
        Hide();
        HidePressKeyToRebind();
        
    }

    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show(Action OnCloseAction)
    {
        this.OnCloseAction = OnCloseAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisuals()
    {
        soundEffectText.text = "Sound Effects : " + Math.Round(SoundEffectsManager.Instance.GetSoundEffectVolume() * 10f);
        musicText.text = "Music : " + Math.Round(MusicManager.Instance.GetVolume() * 10f);

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
    }

    private void ShowPressKeyToRebind()
    {
        pressToRebindUI.gameObject.SetActive(true);
    }

    private void HidePressKeyToRebind()
    {
        pressToRebindUI.gameObject.SetActive(false);
    }

    private void RebindBindings(GameInput.Bindings binding)
    {
        ShowPressKeyToRebind();
        GameInput.Instance.RebindBindings(binding, ()=> {
                HidePressKeyToRebind();
                UpdateVisuals();
            });
       

    }


}
