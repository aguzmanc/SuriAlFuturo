using UnityEngine;
using System.Collections;

public class CreditUp : MonoBehaviour 
{
    public float Velocity = 10f;

    RectTransform _tr;
    float _iniY;
    float _offset;

	// Use this for initialization
	void Start () {
        _tr = GetComponent<RectTransform>();
        _iniY = _tr.position.y;
        _offset = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        _offset += Time.deltaTime * Velocity;

        Vector3 p = _tr.position;
        _tr.position = new Vector3(p.x, _iniY + _offset, p.z);
	    
	}
}
