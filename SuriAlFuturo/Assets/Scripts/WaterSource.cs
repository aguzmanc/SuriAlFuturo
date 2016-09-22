using UnityEngine;
using System.Collections;


public class WaterSource : MonoBehaviour 
{
    public enum Tag {none,a,b,c,d,e,f};

    public WaterSource.Tag WaterSourceTag = Tag.none;

    private ParticleSystem _waterFlow;
    private bool _isWaterOn;
    private bool _canInteract;
    private TimeTravelController _timeTravelController;

	void Start () 
    {
        _waterFlow = GetComponentInChildren<ParticleSystem>();
        _isWaterOn = true;
        _canInteract = false;

        _timeTravelController = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).GetComponent<TimeTravelController>();
        _timeTravelController.OnWaterSourceLoad(this);
	}
	

	void Update () 
    {        
        // open and close water flow
        if(Input.GetButtonDown("Interact") && _canInteract) {

            if(_isWaterOn)
                _waterFlow.Stop();
            else
                _waterFlow.Play();

            _isWaterOn = !_isWaterOn;

            _timeTravelController.OnWaterSourceToggled(WaterSourceTag, _isWaterOn);
        }
	}


    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Entrando a water source: " + coll.name);
        _canInteract = true;
    }

    void OnTriggerExit(Collider coll)
    {
        _canInteract = false;
    }


    public void SwitchWaterFlow(bool isOn)
    {
        _isWaterOn = isOn;

        if(_isWaterOn)
            _waterFlow.Play();
        else
            _waterFlow.Stop();
        
    }


}
