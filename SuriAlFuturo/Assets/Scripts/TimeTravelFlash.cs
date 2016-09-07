using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class TimeTravelFlash : MonoBehaviour 
{
    [Range(0.01f, 0.3f)]
    public float DisableFlashVel = 0.01f;

    float _alpha;
    Image _img;
    Color _originalColor;
    bool _disabling = false;


	void Start () 
    {
        _alpha = 0;
        _img = GetComponent<Image>();
        _originalColor = new Color(_img.color.r, _img.color.g, _img.color.b);

        _img.color = new Color(
            _originalColor.r,
            _originalColor.g,
            _originalColor.b,
            0);
	}
	
	void Update () 
    {
        if(false == _disabling)
            return;

        _alpha -= DisableFlashVel * Time.deltaTime;

        if(_alpha < 0) {
            _alpha = 0;
            _disabling = false;
        }

        _img.color = new Color(
            _originalColor.r,
            _originalColor.g,
            _originalColor.b,
            _alpha);
	}



    public void Flash() 
    {
        _disabling = true;
        _alpha = 1;
    }
}
