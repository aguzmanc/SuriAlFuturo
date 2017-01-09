using UnityEngine;
using System.Collections;

public class PerlinTransformations : MonoBehaviour 
{
    PerlinTransformation [] _perlins;

    void Start() 
    {
        _perlins = GetComponents<PerlinTransformation>();
    }


    void Update() 
    {
        float [] transformations = new float[6]{0,0,0,0,0,0};

        for(int i=0;i<_perlins.Length;i++) {
            PerlinTransformation p = _perlins[i];

            if(p.PosX) transformations[0] = p.Value;
            if(p.PosY) transformations[1] = p.Value;
            if(p.PosZ) transformations[2] = p.Value;
            if(p.RotX) transformations[3] = p.Value;
            if(p.RotY) transformations[4] = p.Value;
            if(p.RotZ) transformations[5] = p.Value;
        }

        transform.localPosition = new Vector3(transformations[0],transformations[1],transformations[2]);
        transform.localRotation = Quaternion.Euler(new Vector3(transformations[3],transformations[4],transformations[5]));
    }
}
