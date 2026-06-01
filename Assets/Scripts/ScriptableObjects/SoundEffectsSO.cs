using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffectsSO", menuName = "Scriptable Objects/SoundEffectsSO")]
public class SoundEffectsSO : ScriptableObject
{
    public AudioClip[] chopSFXs;
    public AudioClip[] deliveryFailSFXs;
    public AudioClip[] deliverySuccessSFXs;
    public AudioClip[] footStepsSFXs;
    public AudioClip[] objectDropSFXs;
    public AudioClip[] objectPickupSFXs;
    public AudioClip sizzleSFXs;
    public AudioClip[] trashSFXs;
    public AudioClip[] warningSFXs;
}
