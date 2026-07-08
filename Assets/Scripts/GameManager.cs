using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public event EventHandler OnStateChanged;
    


    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
        PlayingMinigame,
    }
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimerMax = 60f;
    private float gamePlayingTimer;
    private bool isGamePaused = false;
    
    private State state;


    private void Awake()
    {
        Instance = this;
        gamePlayingTimer = gamePlayingTimerMax;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseGame += GameInput_OnPauseGame;
        GameInput.Instance.OnInteraction += GameInput_OnInteraction;
        DeliveryManager.Instance.OnOrderSuccess += Delivery_OnOrderSuccess;
    }

    public void StopMinigame()
    {
        state = State.GamePlaying;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartMinigame()
    {
        state = State.PlayingMinigame;
        OnStateChanged?.Invoke(this, EventArgs.Empty);

    }

    private void GameInput_OnInteraction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Delivery_OnOrderSuccess(object sender, EventArgs e)
    {
        gamePlayingTimer = gamePlayingTimerMax;
    }

    private void GameInput_OnPauseGame(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if(countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
            case State.PlayingMinigame:

                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsGameCountDown()
    {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsPlayingMinigame()
    {
        return state == State.PlayingMinigame;
    }

    public float GetCountDownTimer()
    {
        return countdownToStartTimer;
    }


    

    public float GetGameplayTimerNormalized()
    {
        return gamePlayingTimer/gamePlayingTimerMax;
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this,EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnPaused?.Invoke(this,EventArgs.Empty);
        }
    }

    

}
