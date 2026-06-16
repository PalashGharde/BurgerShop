using UnityEngine;

public class ProgressBarFlashingUI : MonoBehaviour
{
    private string FLASH_TRIGGER = "ShowFlash";
    private Animator animator;
    [SerializeField] private StoveCounter stoveCounter;

    private bool show = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Start()
    {
        stoveCounter.OnProgressUpdate += StoveCounter_OnProgressUpdate;
        animator.SetBool(FLASH_TRIGGER,false);
    }

    private void StoveCounter_OnProgressUpdate(object sender, IHasProgress.OnProgressUpdateArgs e)
    {
        float burnShowFlashAmount = .5f;
        show = stoveCounter.IsStoveBurning() && (e.progressAmount > burnShowFlashAmount);
        animator.SetBool(FLASH_TRIGGER,show);
    }

}
