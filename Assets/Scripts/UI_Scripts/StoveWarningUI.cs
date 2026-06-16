using System;
using UnityEngine;

public class StoveWarningUI : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;
    private bool show = false;
    private float warningSoundTimer;

    private void Start()
    {
        stoveCounter.OnProgressUpdate += StoveCounter_OnProgressUpdate;
        warningSoundTimer = .5f;
        Hide();
    }

    private void Update()
    {
        PlayWarningSound();
    }




    private void StoveCounter_OnProgressUpdate(object sender, IHasProgress.OnProgressUpdateArgs e)
    {
        float burnShowFlashAmount = .5f;
        show = stoveCounter.IsStoveBurning() && (e.progressAmount > burnShowFlashAmount);
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void PlayWarningSound()
    {
        if (show)
        {
            warningSoundTimer-=Time.deltaTime;
            if (warningSoundTimer < 0)
            {
                warningSoundTimer = .2f;
                SoundEffectsManager.Instance.PlayWarningSound(gameObject.transform.position);
            }
        }
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
