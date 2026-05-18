using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnVisual;
    [SerializeField] private GameObject sizzlingParticles;

    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChanged += TurnOnOffVisuals;
    }

    private void TurnOnOffVisuals(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if( e.state == StoveCounter.FryingState.Frying ||  e.state == StoveCounter.FryingState.Burning)
        {
            ShowVisuals();
        }
        else
        {
            HideVisuals();
        }
    }

    private void ShowVisuals()
    {
        stoveOnVisual.SetActive(true);
        sizzlingParticles.SetActive(true);
    }

    private void HideVisuals()
    {
        stoveOnVisual.SetActive(false);
        sizzlingParticles.SetActive(false);
    }
}
