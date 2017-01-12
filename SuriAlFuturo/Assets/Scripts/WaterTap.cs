using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class WaterTap : MonoBehaviour 
{
    public Vector3 PersistenceKey;
    public AudioSource WaterFlowingSound;


    public bool IsOn {
        get {
            return _isOn;
        }
    }

    public bool IsUsable {
        get{
            return _isUsable;
        }
    }


    private ParticleSystem _fountain;
    public bool _isOn;
    public bool _isUsable;
    public bool _canInteract;
    private bool _interactionTriggered;

    private TimeTravelController _timeTravelController;
    private TapController _tapController;
    private GameController _gameController;

    void Start () {
        _isOn = true;
        _isUsable = GetComponent<Collider>().enabled;
        PersistenceKey = this.transform.position;

        _fountain = GetComponentInChildren<ParticleSystem>();
        _gameController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<GameController>();
        _timeTravelController = _gameController.GetComponent<TimeTravelController>();
        _tapController = _gameController.GetComponent<TapController>();

        _tapController.Taps.Add(this);
        _tapController.Load(this);
    }
    
    void Update () {
        if (Input.GetButtonDown("Interact") || _interactionTriggered) {
            _interactionTriggered = false;
            if (_canInteract) {
                ToggleFountain(!_isOn, false);
            }
        }
    }

    void OnTriggerEnter (Collider c) 
    {
        if(c.tag==Tag.Player){
            _gameController.CanUseTap = _canInteract = true;
        }
    }

    void OnTriggerExit (Collider c) {
        _gameController.CanUseTap = _canInteract = false;
    }

    void OnDestroy () {
        _tapController.Save(this);
        _tapController.Taps.Remove(this);
    }

    public void ToggleFountain (bool value, bool forceNotifyUI) 
    {
        _isOn = value;

        if(_isOn) {
            _fountain.Play();
            _gameController.GetComponent<SFXController>().PlayWaterOn();
            WaterFlowingSound.UnPause();

        } else {
            _fountain.Stop();
            _gameController.GetComponent<SFXController>().PlayWaterOff();
            WaterFlowingSound.Pause();

            if(_canInteract || forceNotifyUI){ // only when Suri is close
                _gameController.GetComponent<TapController>().NotifyTapOff();
            }
        }

        // disables interaction with tap after using it.
        _gameController.CanUseTap = _canInteract = false;
    }

    public void TriggerInteraction () {
        _interactionTriggered = true;
    }

    public void EnableUsable() 
    {
        _isUsable = true;
    }

    #region PERSISTENCE
    public PersistedWaterSource GetPersistedObject () 
    {
        return new PersistedWaterSource(_isOn, _isUsable);
    }

    public void Load (bool isOn, bool isUsable) {
        ToggleFountain(isOn, false);
        _isUsable = isUsable;
        GetComponent<Collider>().enabled = isUsable;
    }
    #endregion
}
