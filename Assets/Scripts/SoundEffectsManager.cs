using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundEffectsManager : MonoBehaviour
{
    private string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectVolume";
    public static SoundEffectsManager Instance {get; private set;}
    [SerializeField] private SoundEffectsSO soundEffectsSO;

    private float soundEffectVolume= 1.0f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManager_OnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailure += DeliveryManager_OnOrderFailure;
        CuttingCounter.OnCutAction += CuttingCounter_OnCutAction;
        Player.Instance.OnObjectPickup += Player_OnObjectPickup;
        BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;

        SodaMachineUI.OnItemPickup += Player_OnObjectPickup;
        SodaMachineUI.OnItemThrow += OnAnyObjectTrashed;
        SodaMachineUI.OnDrinkFilled += SodaMachineUI_OnDrinkFilled;

        soundEffectVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1.0f);
    }

    private void SodaMachineUI_OnDrinkFilled(object sender, EventArgs e)
    {
        PlaySound(soundEffectsSO.sodaFillingSFXs, Player.Instance.transform.position);
    }

    private void OnAnyObjectTrashed(object sender, EventArgs e)
    {
        PlaySound(soundEffectsSO.trashSFXs, Player.Instance.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(soundEffectsSO.trashSFXs, trashCounter.transform.position);
    }

    private void Player_OnObjectPickup(object sender, EventArgs e)
    {
        PlaySound(soundEffectsSO.objectPickupSFXs, Player.Instance.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlaced(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(soundEffectsSO.objectDropSFXs, baseCounter.transform.position);
    }

    private void CuttingCounter_OnCutAction(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(soundEffectsSO.chopSFXs, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnOrderFailure(object sender, EventArgs e)
    {
        Vector3 deliveryCounterPosition = DeliveryCounter.Instance.transform.position;
        PlaySound(soundEffectsSO.deliveryFailSFXs, deliveryCounterPosition);
    }

    private void DeliveryManager_OnOrderSuccess(object sender, EventArgs e)
    {
        Vector3 deliveryCounterPosition = DeliveryCounter.Instance.transform.position;
        PlaySound(soundEffectsSO.deliverySuccessSFXs, deliveryCounterPosition);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * soundEffectVolume);
    } 
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * soundEffectVolume);
    }
    public void PlayFootstepSound(Vector3 position, float volumeMultiplier)
    {
        PlaySound(soundEffectsSO.footStepsSFXs, position, volumeMultiplier * soundEffectVolume);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(soundEffectsSO.warningSFXs[1], position);
    }

    public void PlayCountDownSound()
    {
        PlaySound(soundEffectsSO.warningSFXs[0], Vector3.zero);
    }

    public void ChangeVolume()
    {
        soundEffectVolume += 0.1f;
        if (soundEffectVolume > 1.1f)
        {
            soundEffectVolume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, soundEffectVolume);
    }

    public float GetSoundEffectVolume()
    {
        return soundEffectVolume;
    }
}
