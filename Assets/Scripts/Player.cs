using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance {get; private set;}        //Singleton pattern to get only one player

    [SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.4f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    private Vector3 lastInteractionDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    [SerializeField] private Transform KitchenObjectHoldPoint;


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public event EventHandler OnObjectPickup;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There are more Instance of Player");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteraction += GameInput_OnInteraction;
        gameInput.OnAltInteraction += GameInput_OnAltInteraction;
    }

    private void GameInput_OnAltInteraction(object sender, EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null)
        {
            selectedCounter.AltInteract();
        }
    }

    private void GameInput_OnInteraction(object sender, EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying()) return; 
        
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractionDirection = moveDir;
        }

        float interactDistance = 2f;

        if(Physics.Raycast(transform.position, lastInteractionDirection, out RaycastHit raycastHit, interactDistance))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter counterInFront))
            {
                SetSelectedCounter(counterInFront);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter; // modifying the player's selected counter
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
            selectedCounter = selectedCounter
        });
    }

    private void HandleMovement()
    {
        if(GameManager.Instance.IsPlayingMinigame() || GameManager.Instance.IsGameOver()) return;

        Vector2 inputVector = gameInput.GetInputVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = playerSpeed * Time.deltaTime;

        bool hasMovementInput = moveDir.sqrMagnitude > 0.01f;

        bool canMove = false;

        if (hasMovementInput)
        {
            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDir,
                moveDistance
            );

            if (!canMove)
            {
                // Try moving only on X
                Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;

                canMove = moveDir.x != 0 &&
                        !Physics.CapsuleCast(
                            transform.position,
                            transform.position + Vector3.up * playerHeight,
                            playerRadius,
                            moveDirX,
                            moveDistance
                        );

                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    // Try moving only on Z
                    Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;

                    canMove = moveDir.z != 0 &&
                            !Physics.CapsuleCast(
                                transform.position,
                                transform.position + Vector3.up * playerHeight,
                                playerRadius,
                                moveDirZ,
                                moveDistance
                            );

                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                }
            }

            if (canMove)
            {
                transform.position += moveDir * moveDistance;

                float rotateSpeed = 10f;

                transform.forward = Vector3.Slerp(
                    transform.forward,
                    moveDir,
                    rotateSpeed * Time.deltaTime
                );
            }
        }

        isWalking = hasMovementInput;
    }


    // private void HandleMovement()
    // {
    //     Vector2 inputVector = gameInput.GetInputVectorNormalized();

    //     Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    //     float rotateSpeed = 20f;
    //     transform.forward = Vector3.Slerp(transform.forward, moveDir , rotateSpeed*Time.deltaTime);

    //     float moveDistance = playerSpeed * Time.deltaTime;

    //     bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir,moveDistance);

    //     if (!canMove) // Check if player can move while hugging the wall. (at on of the components of diagonal input)
    //     {
    //         // Check if player can move on only x
    //         Vector3 moveDirX = new Vector3(moveDir.x,0,0); // this is not normalized so the player move slower while hugging the wall
    //         canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX ,moveDistance);

    //         if (canMove)
    //         {
    //             moveDir = moveDirX;
    //         }
    //         else // Check if player can move on only z
    //         {
    //             Vector3 moveDirZ = new Vector3(0,0,moveDir.z); 
    //             canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ ,moveDistance);

    //             if (canMove)
    //             {
    //                 moveDir = moveDirZ;
    //             }
    //             else
    //             {
    //                 //player cannot move
    //             }
    //         }


    //     }

    //     if (canMove)
    //     {
    //         transform.position += moveDir * playerSpeed * Time.deltaTime ;
    //     }

    //     isWalking = moveDir != Vector3.zero;
    // }

    public bool HasPlate()
    {
        return GetKitchenObjectOnTop().TryGetPlate(out PlateKitchenObject plate);
    }

    
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public KitchenObject GetKitchenObjectOnTop()
    {
        return kitchenObject;
    }

    public void SetKitchenObjectOnTop(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnObjectPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public Transform GetKitchenObjectParentTop()
    {
        return KitchenObjectHoldPoint;
    }


    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
}
