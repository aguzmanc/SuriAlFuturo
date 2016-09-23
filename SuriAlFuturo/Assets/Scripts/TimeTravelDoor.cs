using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimeTravelDoor : MonoBehaviour 
{
    public WindZone Wind;
    public Light CenterLight;
    public int SecondsAfterTravelDisabled = 10;
    public TimeTravelFlash UIFlash;

    private bool _timeTravelEnabled;
    private ParticleSystem _particles;
    private UnityStandardAssets.Utility.AutoMoveAndRotate _autoRotate;
    private TimeTravelController _timeTravelController;


	void Start () 
    {
        _particles = GetComponent<ParticleSystem>();
        _autoRotate = GetComponent<UnityStandardAssets.Utility.AutoMoveAndRotate>();
        _timeTravelController = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).GetComponent<TimeTravelController>();

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

            // workaround for SceneManager.UnloadScene inside triggers
            StartCoroutine(DeferedTimeTravel());
        }
    }



    IEnumerator TimeTravelCycle()
    {
        // initial values
        _timeTravelEnabled = true;
        Wind.windMain = 0f;
        CenterLight.enabled = true;
        _particles.emissionRate = 30f;
        _autoRotate.rotateDegreesPerSecond.value = new Vector3(0,0,20);

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
            _autoRotate.rotateDegreesPerSecond.value = new Vector3(0,0,20);

            yield return new WaitForSeconds(5f);

            // reenabling time travel
            _timeTravelEnabled = true;
            CenterLight.enabled = true;
        }
    }


    IEnumerator DeferedTimeTravel()
    {
        yield return new WaitForEndOfFrame();

        _timeTravelController.OnTimeTravel();
    }
}
