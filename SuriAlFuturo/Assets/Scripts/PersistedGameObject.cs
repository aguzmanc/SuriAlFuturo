using UnityEngine;
using System.Collections;

[System.Serializable]
public class PersistedGameObject 
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public bool Enabled;


    public PersistedGameObject (Vector3 position, Quaternion rotation, Vector3 scale, bool enabled) 
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
        Enabled = enabled;
    }
}
