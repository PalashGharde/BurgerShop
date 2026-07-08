using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SodaMachineUI : MonoBehaviour
{
    public static SodaMachineUI Instance {get;set;}
    // [SerializeField] private SodaCounter sodaCounter;
    [SerializeField] private Button cupsStack;
    [SerializeField] private Button cupLidsStack;
    [SerializeField] private Button trashCan;
    [SerializeField] private Button drinkOption1;
    [SerializeField] private Button drinkOption2;
    [SerializeField] private Button drinkOption3;
    [SerializeField] private Button drinkOption4;
    [SerializeField] private Button closeButton;


    [SerializeField] private Image cupInHand;
    [SerializeField] private Image cupInHandLid;
    [SerializeField] private Image cupInHandImageContainer;
    [SerializeField] private Image drinkOption1Cup;
    [SerializeField] private Image drinkOption2Cup;
    [SerializeField] private Image drinkOption3Cup;
    [SerializeField] private Image drinkOption4Cup;

    [SerializeField] private GameObject cupGoToHand;
    [SerializeField] private GameObject cupGoToTrash;
    [SerializeField] private GameObject cupLidGoToHand;

    [SerializeField] private KitchenObjectSO[] drinksObjectSOs;

    public static event EventHandler OnItemPickup;
    public static event EventHandler OnItemThrow;
    public static event EventHandler OnDrinkFilled;


    public static event EventHandler<OnMiniGameArgs> OnFinalSodaMade;

    public class OnMiniGameArgs : EventArgs
    {
        public KitchenObjectSO drink;
    }

    private float waitTimer = 3f;
    private KitchenObjectSO finalDrink;


    private bool isCupInHand = false;
    private bool isCupFilled = false;

    private void Awake()
    {
        Instance = this;
        cupGoToHand.SetActive(false);
        cupGoToTrash.SetActive(false);
        cupLidGoToHand.SetActive(false);
    }


    private void Start()
    {
        cupsStack.onClick.AddListener(()=> {
            IntializeCupInHand();
        });

        cupLidsStack.onClick.AddListener(()=> {
            GiveCupToPlayer();
        });

        trashCan.onClick.AddListener(() =>
        {
            ThrowCupInTrash();
        });

        closeButton.onClick.AddListener(() =>
        {
            DestroyCupInHand();
            ResetAnimationGameObjects();
            StopMinigame();
            Hide();

        });

        drinkOption1.onClick.AddListener(() =>
        {
            int drinkNumber =1;
            FillCupWithDrink(drinkOption1,drinkOption1Cup,drinkNumber);
        });

        drinkOption2.onClick.AddListener(() =>
        {
            int drinkNumber =2;
            FillCupWithDrink(drinkOption2,drinkOption2Cup,drinkNumber);
        });
        drinkOption3.onClick.AddListener(() =>
        {
            int drinkNumber =3;
            FillCupWithDrink(drinkOption3,drinkOption3Cup,drinkNumber);
        });
        drinkOption4.onClick.AddListener(() =>
        {
            int drinkNumber =4;
            FillCupWithDrink(drinkOption4,drinkOption4Cup,drinkNumber);
        });

        Hide();
    }

 

    public void SodaCounter_OnSodaInteract()
    {
        StartMinigame();
    }

    private void IntializeCupInHand()
    {
        if(!isCupInHand)
        {
            isCupInHand = true;
            isCupFilled = false;
            float waitTiming = .2f;
            cupGoToHand.SetActive(true);
            cupGoToTrash.SetActive(false);
            OnItemPickup?.Invoke(this,EventArgs.Empty);
            StartCoroutine(WaitForCupGoInHand(waitTiming));

        }
    }

    private IEnumerator WaitForCupGoInHand(float waitTiming)
    {
        
        yield return new WaitForSeconds(waitTiming);
        cupInHand.gameObject.SetActive(true);
        cupInHandLid.gameObject.SetActive(false);
        cupInHandImageContainer.gameObject.SetActive(false);
        cupGoToHand.SetActive(false);

    }

    private void ThrowCupInTrash()
    {
        if(isCupInHand)
        {
            cupGoToTrash.SetActive(true);
            float waitTiming = .2f;
            DestroyCupInHand();
            StartCoroutine(ThrowCupInHandWaitTimer(waitTiming));
            
        }
    }

    private IEnumerator ThrowCupInHandWaitTimer(float waitTiming)
    {
        
        yield return new WaitForSeconds(waitTiming);
        OnItemThrow?.Invoke(this,EventArgs.Empty);
        ResetAnimationGameObjects();

    }

    private void ShowCupInHand()
    {
        cupInHand.gameObject.SetActive(true);
    }

    private void HideCupInHand()
    {
        cupInHand.gameObject.SetActive(false);
    }

    private void FillCupWithDrink(Button drinkOption, Image drinkCup, int drinkNumber)
    {
        if (!isCupInHand) return;
        if (isCupFilled) return;
        waitTimer = 2f;
        finalDrink = drinksObjectSOs[drinkNumber-1];
        isCupInHand = false;
        OnDrinkFilled?.Invoke(this,EventArgs.Empty);
        StartCoroutine(FillCupWithDrinkRoutine(drinkOption, drinkCup));
    }

    private IEnumerator FillCupWithDrinkRoutine(Button drinkOption, Image drinkCup)
    {
        

        HideCupInHand();

        drinkCup.gameObject.SetActive(true);

        yield return new WaitForSeconds(waitTimer);
        isCupFilled = true;
        drinkCup.gameObject.SetActive(false);

        cupInHandImageContainer.sprite = drinkOption.image.sprite;
        cupInHandImageContainer.gameObject.SetActive(true);
        isCupInHand = true;

        ShowCupInHand();
    }


    private void GiveCupToPlayer()
    {
        if (isCupFilled)
        {
            cupLidGoToHand.SetActive(true);
            cupInHandLid.gameObject.SetActive(true);
            //Play animation
            waitTimer = .5f;
            StartCoroutine(GiveCupToPlayerRoutine());
        }
        StopMinigame();
        
        
    }

    private IEnumerator GiveCupToPlayerRoutine()
    {
        yield return new WaitForSeconds(waitTimer);
        
        OnFinalSodaMade?.Invoke(this,new OnMiniGameArgs
        {
            drink = finalDrink
        });
        

        cupLidGoToHand.SetActive(false);
        DestroyCupInHand();
        Hide();
        OnItemPickup?.Invoke(this,EventArgs.Empty);
        ResetAnimationGameObjects();
        
    }
    private void StopMinigame()
    {
        GameManager.Instance.StopMinigame();
    }

    private void DestroyCupInHand()
    {
        isCupInHand = false;
        cupInHand.gameObject.SetActive(false);
        cupInHandImageContainer.gameObject.SetActive(false);
        isCupFilled = false;
    }

    private void ResetAnimationGameObjects()
    {
        cupGoToTrash.SetActive(false);
        cupGoToHand.SetActive(false);
        cupLidGoToHand.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        cupsStack.Select();
        GameManager.Instance.StartMinigame();
    }
    private void Hide()
    {
        gameObject.SetActive(false); 
    }

    private void StartMinigame()
    {
        Show();
        isCupInHand = false;
        isCupFilled = false;
        cupInHand.gameObject.SetActive(false);
        cupInHandImageContainer.gameObject.SetActive(false);
        drinkOption1Cup.gameObject.SetActive(false);
        drinkOption2Cup.gameObject.SetActive(false);
        drinkOption3Cup.gameObject.SetActive(false);
        drinkOption4Cup.gameObject.SetActive(false);
    }

    public static void ResetStaticData()
    {
        OnItemPickup = null;
        OnItemThrow = null;
        OnDrinkFilled = null;
        OnFinalSodaMade = null;
    }

}
