using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    private bool _drivingBoat;
    public FollowCamera FollowingCamera;

    public void SetDrivingBoat (bool value) {
        _drivingBoat = value;
        if (value) {
            FollowingCamera.EnterWater();
        } else {
            FollowingCamera.ExitWater();
        }
    }

}
