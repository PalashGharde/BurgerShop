using System;
using UnityEngine;

public class DeepFryVisualAndSound : MonoBehaviour
{
    [SerializeField] GameObject sizzlingParticles;
    [SerializeField] DeepFryCounter deepFryCounter;

    private AudioSource audioSource;

    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        deepFryCounter.OnStateChanged += DeepFryCounter_OnStateChanged;
    }

    private void DeepFryCounter_OnStateChanged(object sender, DeepFryCounter.OnStateChangedEventArgs e)
    {
        if(e.state == DeepFryCounter.DeepFryState.Frying || e.state == DeepFryCounter.DeepFryState.Burning)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        audioSource.Play();
        sizzlingParticles.SetActive(true);
    }

    private void Hide()
    {
        audioSource.Pause();
        sizzlingParticles.SetActive(false);
    }
}
