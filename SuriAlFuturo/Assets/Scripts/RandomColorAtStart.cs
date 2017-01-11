using UnityEngine;
using System.Collections;

public class RandomColorAtStart : MonoBehaviour {
    void Start () {
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_Color", Random.ColorHSV(0,1,
                                                         .5f, .7f,
                                                         1, 1));
        rend.material.SetColor("_EmissionColor", Color.white);
    }
    
    void Update () {
	
    }
}
