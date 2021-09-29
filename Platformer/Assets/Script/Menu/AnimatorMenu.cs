using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatorMenu : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private string startSceneName;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MenuAnimation(int numberAnimation)
    {
        animator.SetInteger("Selected", numberAnimation);
    }

    public void AnimationMenuNumber(int id)
    {
        MenuAnimation(id + 1);

        if (id + 1 == 5)
        {
            Application.Quit();
            Debug.Log("Exit");
        }

        if (id + 1 == 1)
        {
            LevelLoader.SwitchToScene(startSceneName);
            Debug.Log("Start");
        }
    }
}
