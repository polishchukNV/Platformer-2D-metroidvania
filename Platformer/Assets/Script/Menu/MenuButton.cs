using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private MenuButtonController buttonController;
    [SerializeField] private AnimatorMenu animatorMenu;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorFunction animatorFunction;
    [SerializeField] private int thisIndex;
    [SerializeField] private bool activePresset = true;
    
    private void Update()
    {
        ButtonConroller();
    }

    private void ButtonConroller()
    {
        if (buttonController.index == thisIndex)
        {
            animator.SetBool("Selected", true);

            if (Input.GetAxis("Submit") >= 0.7f && activePresset)
            {
                animator.SetBool("Pressed", true);
                animatorMenu.AnimationMenuNumber(thisIndex);
            }
            else if (animator.GetBool("Pressed"))
            {
                animator.SetBool("Pressed", false);
            }
        }
        else
        {
            animator.SetBool("Selected", false);
        }
    }
}
