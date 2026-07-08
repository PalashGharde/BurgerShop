using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    
    private const string IS_WALKING = "IsWalking";
    private const string IS_PICKING_PLATE = "IsPickingPlate";
    private const string IS_PICKING_OBJECT = "IsPickingObject";

    [SerializeField] private Player player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
  



    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_PICKING_PLATE, player.HasKitchenObject() && player.HasPlate());
        animator.SetBool(IS_PICKING_OBJECT, player.HasKitchenObject() && !player.HasPlate());
    }
}
