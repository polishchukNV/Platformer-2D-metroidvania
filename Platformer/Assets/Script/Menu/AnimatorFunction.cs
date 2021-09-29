using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunction : MonoBehaviour
{
    [SerializeField] MenuButtonController buttonController;

    private void PlaySound(AudioClip whichSound)
    {
      buttonController.audioSource.PlayOneShot(whichSound);
    }

}
