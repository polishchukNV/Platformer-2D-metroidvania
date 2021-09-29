using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] private bool isBack;
    [SerializeField] private int minIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private int backButtonIndex;
    public int index;

    private bool inOptions;
    private bool keyDown;
    
    [SerializeField] private AnimatorMenu animatorMenu;
    [SerializeField] private OptionsController optionsController;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        optionsController = OptionsController.Instance;
    }

    private void Update()
    {
        if(optionsController != null)
        {
          inOptions = optionsController.isOptions;
        }

        if (!inOptions)
        {
            ButtonController();

            if (animatorMenu != null)
                BackButton();
        }

    }

    private void BackButton()
    {
        if (Input.GetButtonDown("Cancel") && isBack)
        {
            animatorMenu.AnimationMenuNumber(backButtonIndex);
        }
    }

    private void ButtonController()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0f)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = minIndex;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0f)
                {
                    if (index > minIndex)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }

}
