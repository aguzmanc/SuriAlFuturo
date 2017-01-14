using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {
    public GameObject DockedShip;
    public GameObject Suri;
    public GameObject DisembarkPlace;
    public GameObject Indicator;
    public bool IsSuriAtPort;

    private GameController _gameController;
    private DockController _dockController;
    private bool _isInterfaceInteractionTriggered;

    private FollowCamera _camera;

    public FollowCamera Cam {
        get {
            if(_camera == null)
                _camera = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.MainCamera).transform.parent.
                    gameObject.GetComponent<FollowCamera>();

            return _camera;
        }
    }



    void Start ()
    {
        _isInterfaceInteractionTriggered = false;

        _gameController = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<GameController>();
        _dockController = _gameController.GetComponent<DockController>();
        _dockController.Docks.Add(this);
        Suri = _gameController.GetComponent<GameController>().Suri;
    }

    void Update () {
        Indicator.SetActive(CanEmbark() || CanDisembark());
        if (Indicator.activeSelf) {
            _dockController.CanUseDock = true;
        }

        if ((Input.GetButtonDown("LoadUnloadToShip") ||
             _isInterfaceInteractionTriggered)) {
            _isInterfaceInteractionTriggered = false;
            if (CanEmbark()) {
                Embark();
            } else if (CanDisembark()) {
                Disembark();
            }
        }
    }

    void OnDestroy () {
        _dockController.Docks.Remove(this);
    }

    void OnTriggerEnter (Collider c) {
        GameObject parent = c.transform.parent.gameObject;

        if (parent.CompareTag(SuriAlFuturo.Tag.Ship)) {
            DockedShip = parent;
        } else {
            IsSuriAtPort = true;
        }
    }

    void OnTriggerExit (Collider c) {
        GameObject parent = c.transform.parent.gameObject;

        if (parent.CompareTag(SuriAlFuturo.Tag.Ship)) {
            DockedShip = null;
        } else {
            IsSuriAtPort = false;
        }

        _dockController.CanUseDock = false;
    }

    public bool CanEmbark () {
        return DockedShip != null && Suri.activeSelf && IsSuriAtPort;
    }

    public bool CanDisembark () {
        return DockedShip != null && !Suri.activeSelf;
    }

    public void Embark () {
        Suri.SetActive(false);

        Chapu chapu = _gameController.GetComponent<GameController>().Chapu.GetComponent<Chapu>();
        chapu.Embark();

        DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = true;
        DockedShip.GetComponent<Ship>().ShrinkCollider();
        Cam.Target = DockedShip;
        _gameController.SetDrivingBoat(true);

        _gameController.GetComponent<SFXController>().PlayEmbark();
    }



    public void Disembark ()
    {
        Suri.transform.position = DisembarkPlace.transform.position;
        Suri.SetActive(true);
        DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = false;
        DockedShip.GetComponent<Ship>().EnlargeCollider();
        DockedShip.transform.position = transform.position + 11.2f * transform.right;
        Cam.Target = Suri;
        _gameController.SetDrivingBoat(false);

        Chapu chapu = _gameController.GetComponent<GameController>().Chapu.GetComponent<Chapu>();
        chapu.gameObject.SetActive(true);
        chapu.Disembark();

        _gameController.GetComponent<SFXController>().PlayDisembark();
    }



    public void TriggerInterfaceInteraction ()
    {
        _isInterfaceInteractionTriggered = true;
    }
}
