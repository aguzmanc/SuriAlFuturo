using UnityEngine;
using UnityEngine.UI;

public class UIStamina : MonoBehaviour
{
    [SerializeField]
    RectTransform _fill;


    void Start()
    {
        GameController.onStaminaChanged += OnStaminaChanged;
    }


    void OnDestroy()
    {
        GameController.onStaminaChanged -= OnStaminaChanged;
    }

    // 0 to 1  (no stamina to full stamina)
    void OnStaminaChanged(float stamina)
    { 
        _fill.anchorMax = new Vector2(stamina, 1);    
    }
}
