using UnityEngine;
using System.Collections;

public class TimeTraverLight : MonoBehaviour 
{
    Vector2 _pos;
    Light _light;

    public Vector2 MaxLimits;
    public Vector2 Velocity;
    public float Multiplier = 8f;


	void Start () 
    {
        _pos = new Vector2(MaxLimits.x/2f, MaxLimits.y/2f);
        _light = GetComponent<Light>();
	}
	
	void Update () 
    {
        _pos.x += Velocity.x;
        if(_pos.x > MaxLimits.x || _pos.x < 0) { 
            Velocity.x *= -1f;
        }

        _pos.y += Velocity.y;
        if(_pos.y > MaxLimits.y || _pos.y < 0) {
            Velocity.y *= -1f;
        }
    
        _light.intensity = Mathf.PerlinNoise(_pos.x, _pos.y) * Multiplier;
	}
}
