using UnityEngine;
using System.Collections;

public class TimeTravelDoor : MonoBehaviour 
{
    public WindZone Wind;
    public Light CenterLight;
    public int SecondsAfterTravelDisabled = 10;
    public GameObject PresentTerrain;
    public GameObject FutureTerrain;
    public TimeTravelFlash UIFlash;

    bool _timeTravelEnabled;
    ParticleSystem _particles;
    GameObject _currentTerrain;
    UnityStandardAssets.Utility.AutoMoveAndRotate _autoRotate;

	void Start () 
    {
        _currentTerrain = PresentTerrain;
        PresentTerrain.SetActive(true);
        FutureTerrain.SetActive(false);

        _particles = GetComponent<ParticleSystem>();
        _autoRotate = GetComponent<UnityStandardAssets.Utility.AutoMoveAndRotate>();
        StartCoroutine(TimeTravelCycle());
	}
	
	void Update () 
    {
	
	}


    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag.CompareTo("Player") != 0)
            return;

        if(_timeTravelEnabled)
        {
            _timeTravelEnabled = false;

            UIFlash.Flash();

            if(_currentTerrain == PresentTerrain) {
                PresentTerrain.SetActive(false);
                FutureTerrain.SetActive(true);
                _currentTerrain = FutureTerrain;
            } else {
                PresentTerrain.SetActive(true);
                FutureTerrain.SetActive(false);
                _currentTerrain = PresentTerrain;
            }
        }
    }



    IEnumerator TimeTravelCycle()
    {
        // initial values
        _timeTravelEnabled = true;
        Wind.windMain = 0f;
        CenterLight.enabled = true;
        _particles.emissionRate = 30f;
        _autoRotate.rotateDegreesPerSecond.value = new Vector3(0,20,0);

        while(true)
        {
            yield return new WaitWhile( () => (_timeTravelEnabled));
            // disabling time travel
            Wind.windMain = 10f;
            _particles.emissionRate = 0f;
            _autoRotate.rotateDegreesPerSecond.value = new Vector3(0,0,0);
            CenterLight.enabled = false;

            yield return new WaitForSeconds(SecondsAfterTravelDisabled);
            // reenabling time travel door (only aspect)
            Wind.windMain = 0f;
            _particles.emissionRate = 30f;
            _autoRotate.rotateDegreesPerSecond.value = new Vector3(0,20,0);

            yield return new WaitForSeconds(5f);

            // reenabling time travel
            _timeTravelEnabled = true;
            CenterLight.enabled = true;
        }
    }
}
