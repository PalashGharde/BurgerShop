using System;
using TMPro;
using UnityEngine;

public class CountDownTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownTimer;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameCountDown())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        countDownTimer.gameObject.SetActive(false);
    }

    private void Show()
    {
        countDownTimer.gameObject.SetActive(true);
    }

    private void Update()
    {
        countDownTimer.text = Math.Ceiling(GameManager.Instance.GetCountDownTimer()).ToString();
    }
}
