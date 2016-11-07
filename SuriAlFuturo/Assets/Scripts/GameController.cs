using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    private bool _drivingBoat;
    public FollowCamera FollowingCamera;
    public GameObject Suri;

    public GameObject ControlledCharacter;

    public void SetDrivingBoat (bool value) {
        _drivingBoat = value;
        if (value) {
            FollowingCamera.EnterWater();
        } else {
            FollowingCamera.ExitWater();
        }
    }

}
