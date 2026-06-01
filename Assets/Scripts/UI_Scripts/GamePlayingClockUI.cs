using UnityEngine;
using UnityEngine.UI;
using System;

public class GamePlayingTimerUI : MonoBehaviour
{
    [SerializeField] private Image timerClockImage;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void Update()
    {
        timerClockImage.fillAmount = GameManager.Instance.GetGameplayTimerNormalized();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
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
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
