using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private const string CUT="Cut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnProgressUpdate += CuttingCounter_OnProgressUpdate;
    }

    private void CuttingCounter_OnProgressUpdate(object sender, IHasProgress.OnProgressUpdateArgs e)
    {
        if (e.playAnimation)
        {
            animator.SetTrigger(CUT);
        }
    }
}
