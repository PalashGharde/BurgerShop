using System;
using TMPro;
using UnityEngine;

public class CountDownTimerUI : MonoBehaviour
{

    private const string COUNTDOWN_TRIGGER = "CountDownTrigger";
    [SerializeField] private TextMeshProUGUI countDownTimer;
    private Animator animator;
    private int previousCountDownNum = 10;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        
    }

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
        int countDownNum = (int)Math.Ceiling(GameManager.Instance.GetCountDownTimer());
        countDownTimer.text = countDownNum.ToString();

        if(countDownNum != previousCountDownNum && countDownNum>0)
        {
            previousCountDownNum = countDownNum;
            animator.SetTrigger(COUNTDOWN_TRIGGER);
            SoundEffectsManager.Instance.PlayCountDownSound();
        }
    }
}
