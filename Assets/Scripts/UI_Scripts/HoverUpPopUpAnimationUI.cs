using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUpPopUpAnimationUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private const string IS_HOVERED = "IsHovered";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHovered(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHovered(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetHovered(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetHovered(false);
    }

    private void SetHovered(bool isHovered)
    {
        if (animator != null)
        {
            animator.SetBool(IS_HOVERED, isHovered);
        }
    }
}
