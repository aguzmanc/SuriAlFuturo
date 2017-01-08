using UnityEngine;
using System.Collections;

public class PerlinTransformation : MonoBehaviour 
{
    Vector2 _posPerlin;
    Vector2 _vPerlin;

    public float Velocity = 0.1f;
    public float Amplitude = 3f;
    public float Offset = 0f;

    public bool PosX = false;
    public bool PosY = false;
    public bool PosZ = false;

    public bool RotX = false;
    public bool RotY = false;
    public bool RotZ = false;

    public float Value;

    void Start () 
    {
        _posPerlin = new Vector2();
        _vPerlin = Random.insideUnitCircle;
    }



    void Update () 
    {
        Value = ((Mathf.PerlinNoise(_posPerlin.x, _posPerlin.y) * Amplitude * 2) - Amplitude) + Offset;
        _posPerlin += (_vPerlin * Velocity);

        //transform.localPosition = new Vector3(PosX ? noise : 0, PosY ? noise : 0, PosZ ? noise : 0);
        //transform.localRotation = Quaternion.Euler( new Vector3(RotX?noise:0,RotY?noise:0,RotZ?noise:0));
    }
}
