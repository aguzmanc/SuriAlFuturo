using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
    public bool IsInHole;
    public GameObject Target;
    public float NoHoleDistance = 15;
    public float HoleDistance = 25;
    public GameObject TheCamera;

    public Vector3 NoHoleRotation = new Vector3(60, 45, 0);
    public Vector3 HoleRotation = new Vector3(90, 45, 0);
    public float TransitionTime = 5;

    private float _timeOnState = 0;


    #region Properties

    // read only property
    public Vector3 Forward
    {
        get {
            float y = IsInHole ? HoleRotation.y : NoHoleRotation.y;

            return new Vector3(Mathf.Cos(((90-y)/360) * 2 * Mathf.PI), 0,
                Mathf.Cos((y/360) * 2 * Mathf.PI)); 
        }
    }

    public Vector3 Right
    {
        get {
            return transform.right;
        }
    }


    /** Singleton Instance **/
    private static FollowCamera _instance;
    public static FollowCamera Instance
    {
        get {
            return _instance;
        }
    }

    #endregion



    
    void Start () 
    {
        _instance = this;
        
        TheCamera.transform.localPosition = new Vector3(0,0, -NoHoleDistance);
        transform.rotation = Quaternion.Euler(NoHoleRotation);
    }
    
    void Update () 
    {
        Vector3 a, b;
        float c, d;

        _timeOnState += Time.deltaTime;
        this.transform.position = Target.transform.position;

        if (IsInHole) {
            a = NoHoleRotation;
            b = HoleRotation;
            c = NoHoleDistance;
            d = HoleDistance;
        } else {
            b = NoHoleRotation;
            a = HoleRotation;
            d = NoHoleDistance;
            c = HoleDistance;
        }

        transform.rotation =
            Quaternion.Euler(Vector3.Slerp(a, b, _timeOnState/TransitionTime));

        TheCamera.transform.localPosition =
            new Vector3(0,0, -Mathf.Lerp(c, d, _timeOnState/TransitionTime));
    }

    public void OnHoleEnter () 
    {
        _timeOnState = 0;
        IsInHole = true;
    }

    public void OnHoleExit () 
    {
        _timeOnState = 0;
        IsInHole = false;
    }


}
