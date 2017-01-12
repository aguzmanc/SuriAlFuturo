using UnityEngine;
using System.Collections;

public class ShipAnimation : MonoBehaviour {
    public GameObject Suri;
    public GameObject TotoraShip;
    [HideInInspector]
    public Transform ChapuHolder;

    private CharacterMovement _shipMover;
    private Animator _suriAnimator;
    private Animator _totoraShipAnimator;

    void Start () {
        _shipMover = GetComponent<CharacterMovement>();
        _suriAnimator = Suri.GetComponent<Animator>();
        _totoraShipAnimator = TotoraShip.GetComponent<Animator>();
    }
    
    void Update () {
        Suri.SetActive(_shipMover.IsControlledByPlayer);

        if(_shipMover.IsControlledByPlayer){
            _suriAnimator.SetFloat("Speed", _shipMover.CurrentSpeedPercent);
            _totoraShipAnimator.SetFloat("Speed", _shipMover.CurrentSpeedPercent);
        }
    }
}
