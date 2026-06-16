using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private string SHOW_POP_UP = "ShowPopUp";
    [SerializeField] private Image Background;
    [SerializeField] private Image ResultImage;
    [SerializeField] private TextMeshProUGUI ResultText;

    [SerializeField] private Color DeliverySuccessColor;
    [SerializeField] private Color DeliveryFailColor;

    [SerializeField] private Sprite DeliverySuccessImage;
    [SerializeField] private Sprite DeliveryFailImage;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Start()
    {
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManager_OnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailure += DeliveryManager_OnOrderFailure;
        Hide();
    }

    private void DeliveryManager_OnOrderFailure(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(SHOW_POP_UP);
        Background.color = DeliveryFailColor;
        ResultImage.sprite = DeliveryFailImage;
        ResultText.text = "Delivery\nFail";
    }

    private void DeliveryManager_OnOrderSuccess(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(SHOW_POP_UP);
        Background.color = DeliverySuccessColor;
        ResultImage.sprite = DeliverySuccessImage;
        ResultText.text = "Delivery\nSuccess";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
