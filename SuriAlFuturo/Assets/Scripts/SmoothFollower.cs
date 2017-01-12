using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class SmoothFollower : MonoBehaviour 
{
    public Transform Target;
    public float Smoothness = 0.1f;

    private GameController _controller;
	
	void Start () 
    {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<GameController>();
	}
	
	
	void Update () 
    {
        Transform tg = Target;
        
        if(tg == null && _controller.ControlledCharacter != null){
            tg = _controller.ControlledCharacter.transform;
        }

        if(tg != null) {
            Vector3 to = new Vector3(tg.position.x, transform.position.y, tg.position.z);
            transform.position = Vector3.Lerp(transform.position, to, Smoothness * Time.deltaTime);
        }
	}
}
