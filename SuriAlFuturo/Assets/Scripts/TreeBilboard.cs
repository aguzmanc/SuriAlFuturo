using UnityEngine;
using System.Collections;

public class TreeBilboard : MonoBehaviour 
{
    public Sprite FrontSprite;
    public Sprite LeftSprite;

    SpriteRenderer _rend;


	void Start () 
    {
        _rend = GetComponent<SpriteRenderer>();
	}
	
	void Update () 
    {
        Vector3 diff = Camera.main.transform.position - transform.position;
        if(Mathf.Abs(diff.x) < Mathf.Abs(diff.z)){
            _rend.sprite = FrontSprite;
        } else {
            _rend.sprite = LeftSprite;
        }

        diff = new Vector3(diff.x, 0, diff.z);

        transform.rotation = Quaternion.LookRotation(diff);
	}
}
