using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private NewPlayer target;
    [SerializeField] private Image[] healths;
    [SerializeField] private Sprite activeHealths;
    [SerializeField] private Sprite notHealths;

    private void Start()
    { 
        UpdateValue(NewPlayer.Instance.health);
    }

    private void OnEnable()
    {
        target.OnHealthChange += UpdateValue;
    }

    private void OnDisable()
    {
        target.OnHealthChange -= UpdateValue;
    }

    public void UpdateValue(int health)
    {
        if (target == null) return;
        for (int i = 0; i < healths.Length; i++)
        {
            if (health > i)
            {
                healths[i].sprite = activeHealths;
            }
            else
            {
                healths[i].sprite = notHealths;
            }
        }
    }

}
