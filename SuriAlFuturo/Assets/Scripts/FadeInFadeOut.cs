using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInFadeOut : MonoBehaviour 
{
    Image _img;
    Text _text;
    bool _fadingIn;
    bool _fadingOut;
    float _secondsFading;
    bool _finishedFlag;

    public float SecondsToFade = 2;


	void Start () 
    {
        _fadingIn = false;
        _fadingOut = false;
        _finishedFlag = true;

        _img = GetComponent<Image>();
        _text = GetComponent<Text>();
	}
	


	void Update () 
    {
        if(_fadingIn) {
            float factor = 1f - (_secondsFading/SecondsToFade);

            ChangeFade(_img, factor);
            ChangeFade(_text, factor);

            _secondsFading -= Time.deltaTime;

            if(_secondsFading < 0) {
                _fadingIn = false;
                _finishedFlag = true;

                ChangeFade(_img, 1f);
                ChangeFade(_text, 1f);
            }
        }


        if(_fadingOut){
            float factor = _secondsFading/SecondsToFade;

            ChangeFade(_img, factor);
            ChangeFade(_text, factor);

            _secondsFading -= Time.deltaTime;

            if(_secondsFading < 0) {
                _fadingOut = false;
                _finishedFlag = true;

                ChangeFade(_img, 0f);
                ChangeFade(_text, 0f);
            }
        }
	}



    public void Show()
    {
        ChangeFade(_img, 1f);
        ChangeFade(_text, 1f);

        _fadingOut = false;
        _fadingIn = false;
        _secondsFading = -1;
    }



    public void Hide() 
    {
        ChangeFade(_img, 0f);
        ChangeFade(_text, 0f);

        _finishedFlag = true;
        _fadingOut = false;
        _fadingIn = false;
        _secondsFading = -1;
    }



    public void FadeIn() 
    {
        _secondsFading = SecondsToFade;
        _fadingIn = true;
        _finishedFlag = false;
    }



    public void FadeOut() 
    {
        _secondsFading = SecondsToFade;
        _fadingOut = true;
        _finishedFlag = false;
    }


    // just consume once
    public bool HasFinishedFade {
        get{
            if(false == _finishedFlag)
                return false;

            _finishedFlag = true;
            return true;
        }
    }


    void ChangeFade(Image img, float factor)
    {
        if(img == null)
            return;
        
        Color c = img.color;
        img.color = new Color(c.r, c.g, c.b, factor);

        img.enabled = (factor > 0f);
    }



    void ChangeFade(Text text, float factor)
    {
        if(text == null)
            return;

        Color c = text.color;
        text.color = new Color(c.r, c.g, c.b, factor);

        text.enabled = (factor > 0f);
    }

}
