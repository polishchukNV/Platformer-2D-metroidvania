using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] selectedGameObject;
    [SerializeField] private MenuButtonController menuButtonController;

    public bool isOptions;

    private static OptionsController instance;
    public static OptionsController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<OptionsController>();
            return instance;
        }
    }

    private void Update() => OptionsControl();

    public void OptionsControl()
    {
        if(Input.GetAxis("Horizontal") > 0.2f && WhatElementActive() || Input.GetAxis("Submit") >= 0.7f)
        {
            SelectedElement(selectedGameObject[menuButtonController.index - 5]);
            isOptions = true;
        }

        if (Input.GetAxis("Horizontal") < -0.2f && WhatElementActive() || Input.GetButtonUp("Cancel"))
        {
            SelectedElement(null);
            isOptions = false;
        }
    }

     private bool WhatElementActive()
     {
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null &&
        EventSystem.current.currentSelectedGameObject.GetComponent<Slider>() != null)
        {
            return false;
        }

        return true;
     }

    private void SelectedElement(GameObject gameObject)
    {
        eventSystem.SetSelectedGameObject(gameObject);
    } 

}
