using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [Header("Internal Setup")]
    [SerializeField]
    Image[] _hearths;

    [SerializeField]
    FadeInFadeOut _fade;


    int _index;

    void Start()
    {
        for(int i=0;i<_hearths.Length;i++) {
            _hearths[i].gameObject.SetActive(true);
        }

        _index = _hearths.Length - 1;

        GameController.onDamage += OnDamage;

        GameController.onGameFinished += OnGameFinished;
    }


    void OnDestroy()
    {
        GameController.onDamage -= OnDamage;
        GameController.onGameFinished -= OnGameFinished;
    }



    void OnDamage()
    { 
        if(_index >= 0) {
            _hearths[_index].gameObject.SetActive(false);
            _index --;
        }
    }

    void OnGameFinished()
    { 
        _fade.FadeIn();

        Invoke("RestartGame", 3);
    }

    void RestartGame()
    {
        GameController.RestartGame();
    }
}
