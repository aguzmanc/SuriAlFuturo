using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimeTravelDoor : MonoBehaviour 
{
    public WindZone Wind;
    public Light CenterLight;
    public int SecondsAfterTravelDisabled = 10;
    public TimeTravelFlash UIFlash;

    public GameObject PresentStuff;
    public GameObject FutureStuff;

    bool _timeTravelEnabled;
    ParticleSystem _particles;
    UnityStandardAssets.Utility.AutoMoveAndRotate _autoRotate;

    public bool IsInPresent;


	void Start () 
    {
        SceneManager.LoadScene("Present", LoadSceneMode.Additive);

        IsInPresent = true;

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
        _autoRotate.rotateDegreesPerSecond.value = new Vector3(0,20,0);

//        yield return new WaitForSeconds(4);
//        SceneManager.UnloadScene("Present");


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


    IEnumerator DeferedTimeTravel()
    {
        yield return new WaitForEndOfFrame();

        if(IsInPresent) {
            SceneManager.LoadSceneAsync("Future", LoadSceneMode.Additive);
            SceneManager.UnloadScene("Present");

            IsInPresent = false;
        } else {
            SceneManager.LoadSceneAsync("Present", LoadSceneMode.Additive);
            SceneManager.UnloadScene("Future");

            IsInPresent = true;
        }
        PresentStuff.SetActive(IsInPresent);
        FutureStuff.SetActive(!IsInPresent);
    }
}
