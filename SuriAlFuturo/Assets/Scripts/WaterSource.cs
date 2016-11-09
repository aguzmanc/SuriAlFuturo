using UnityEngine;
using System.Collections;


public class WaterSource : MonoBehaviour 
{
    public enum Tag {none,a,b,c,d,e,f};

    public WaterSource.Tag WaterSourceTag = Tag.none;

    private ParticleSystem _waterFlow;
    private bool _isWaterOn;
    private bool _canInteract;
    private bool _interactionTriggered;
    private TimeTravelController _timeTravelController;
    private GameController _gameController;
    private TapController _tapController;

    void Start () 
    {
        _waterFlow = GetComponentInChildren<ParticleSystem>();
        _isWaterOn = true;
        _canInteract = false;

        _gameController = GameObject.
            FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<GameController>();
        _tapController = _gameController.GetComponent<TapController>();
        _timeTravelController = _gameController.GetComponent<TimeTravelController>();

        _timeTravelController.OnWaterSourceLoad(this);
        _tapController.Taps.Add(this);
    }
	
    
    void Update () 
    {        
        // open and close water flow
        if (Input.GetButtonDown("Interact") || _interactionTriggered) {
            _interactionTriggered = false;
            if (_canInteract) {

                if(_isWaterOn)
                    _waterFlow.Stop();
                else
                    _waterFlow.Play();

                _isWaterOn = !_isWaterOn;

                _timeTravelController.OnWaterSourceToggled(WaterSourceTag, _isWaterOn);
            }
	}
    }


    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Entrando a water source: " + coll.name);
        _gameController.CanUseTap = _canInteract = true;
    }

    void OnTriggerExit(Collider coll)
    {
        _gameController.CanUseTap = _canInteract = false;
    }

    void OnDestroy ()
    {
        _gameController.CanUseTap = false;
        _tapController.Taps.Remove(this);
    }


    public void SwitchWaterFlow(bool isOn)
    {
        _isWaterOn = isOn;

        if(_isWaterOn)
            _waterFlow.Play();
        else
            _waterFlow.Stop();
        
    }

    public void TriggerInteraction () {
        _interactionTriggered = true;
    }
}
