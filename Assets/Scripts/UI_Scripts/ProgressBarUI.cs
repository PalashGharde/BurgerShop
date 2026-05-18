using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectHasProgress;
    [SerializeField] private Image progressBar;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = gameObjectHasProgress.GetComponent<IHasProgress>();

        if(hasProgress == null)
        {
            Debug.LogError("GameObject "+ gameObjectHasProgress +" does not have IHasProgress Component");
        }
        
        hasProgress.OnProgressUpdate += HasProgress_OnProgressUpdate;
        progressBar.fillAmount = 0f;
        HideBar();
    }

    private void HasProgress_OnProgressUpdate(object sender, IHasProgress.OnProgressUpdateArgs e)
    {
        progressBar.fillAmount = e.progressAmount;
        if(progressBar.fillAmount == 0f)
        {
            HideBar();
        }
        else
        {
            ShowBar();
        }
    }

    private void ShowBar()
    {
        gameObject.SetActive(true);
    }

    private void HideBar()
    {
        gameObject.SetActive(false);
    }
}
