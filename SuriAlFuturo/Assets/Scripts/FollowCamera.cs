using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
    public const int IN_FLOOR = 0;
    public const int IN_HOLE = 1;
    public const int IN_WATER = 2;

    public int State = 0;
    public GameObject Target;
    public float FloorDistance = 15;
    public float HoleDistance = 25;
    public float WaterDistance = 30;
    public GameObject TheCamera;

    public Vector3 FloorRotation = new Vector3(60, 45, 0);
    public Vector3 HoleRotation = new Vector3(90, 45, 0);
    public Vector3 WaterRotation = new Vector3(45, 45, 0);
    public float TransitionTime = 5;

    private float _timeOnState = 0;
    private Vector3 _cachedRotation;
    private float _cachedDistance;


    #region Properties

    // read only property
    public Vector3 Forward
    {
        get {
            float y = GetY();

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

        TheCamera.transform.localPosition = new Vector3(0,0, -FloorDistance);
        transform.rotation = Quaternion.Euler(FloorRotation);
    }

    void Update ()
    {
        _timeOnState += Time.deltaTime;
        this.transform.position = Target.transform.position;

        Vector3 rotationOrigin, rotationDestination;
        float distanceOrigin, distanceDestination;

        rotationOrigin = _cachedRotation;
        distanceOrigin = _cachedDistance;


        switch (State) {
            case IN_FLOOR:
                rotationDestination = FloorRotation;
                distanceDestination = FloorDistance;
                break;
            case IN_HOLE:
                rotationDestination = HoleRotation;
                distanceDestination = HoleDistance;
                break;
            default: // IN_WATER
                rotationDestination = WaterRotation;
                distanceDestination = WaterDistance;
                break;
        }
        
        transform.rotation =
            Quaternion.Euler(Vector3.Slerp(rotationOrigin, rotationDestination,
                                           _timeOnState / TransitionTime));

        TheCamera.transform.localPosition =
            new Vector3(0,0, -Mathf.Lerp(distanceOrigin, distanceDestination,
                                         _timeOnState / TransitionTime));
    }

    public void OnHoleEnter ()
    {
        _SwitchState(IN_HOLE);
    }

    public void OnHoleExit ()
    {
        _SwitchState(IN_FLOOR);
    }

    public void EnterWater () {
        _SwitchState(IN_WATER);
    }

    public void ExitWater () {
        _SwitchState(IN_FLOOR);
    }

    private void _SwitchState (int newState) {
        _timeOnState = 0;
        State = newState;
        _cachedRotation = TheCamera.transform.rotation.eulerAngles;
        _cachedDistance = Mathf.Abs(TheCamera.transform.localPosition.z);
    }

    public float GetY () {
        switch (State) {
            case IN_WATER:
                return WaterRotation.y;
                break;
            case IN_FLOOR:
                return FloorRotation.y;
                break;
            case IN_HOLE:
                return HoleRotation.y;
                break;
        }
        return 0;
    }
}
