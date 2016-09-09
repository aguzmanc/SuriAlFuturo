using UnityEngine;
using System.Collections;

public class Suri : MonoBehaviour 
{
    private static GameObject _instance;
    public static GameObject Instance
    {
        get {
            return _instance;
        }
    }


	void Start () 
    {
        _instance = gameObject;
	}
}
