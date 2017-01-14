using UnityEngine;
using System.Collections;

public class Lag : MonoBehaviour {
    public int Iterations;
    public int x;

    void Start () {
        
    }
    
    void Update () {
	for (int i=0; i<Iterations; i++) x++;
    }
}
