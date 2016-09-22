using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class TimeTravelController : MonoBehaviour 
{
    public enum TemporalReality {
        Present,
        NoWaterFuture,
        LittleWaterFuture,
        MediumWaterFuture,
        AlmostFullWaterFuture,
        FullWaterFuture
    };

    [System.Serializable]
    public class TravelTimeRequirement
    {
        public TemporalReality RealityToTravel;
        public List<WaterSource.Tag> WaterSourcesRequired;
    }


    public List<TravelTimeRequirement> TravelRequirements;


    private Dictionary<WaterSource.Tag, bool> _waterSourcesClosed;
    private TemporalReality _currentReality;


    void Awake()
    {
        _waterSourcesClosed = new Dictionary<WaterSource.Tag, bool>();    
    }


	void Start () 
    {
        // check if "Present" scene is in loaded scenes (debug purposes in editor when testing)
        bool isLoaded = false;
        for(int i=0;i<SceneManager.sceneCount;i++){
            string sceneName = SceneManager.GetSceneAt(i).name;
            if(sceneName.CompareTo(TemporalReality.Present.ToString()) == 0) {
                isLoaded = true;
                break;
            }
        }

        if(false == isLoaded)
            SceneManager.LoadScene(TemporalReality.Present.ToString(), LoadSceneMode.Additive);

        _currentReality = TemporalReality.Present; // Suri is in present time at beginning of game
	}


	
	void Update () 
    {
	
	}



    public void OnTimeTravel()
    {
        TemporalReality originReality;
        TemporalReality destinationReality;

        if(_currentReality != TemporalReality.Present) { // is in future.. back to present then
            originReality = _currentReality;
            destinationReality = TemporalReality.Present;
            _currentReality = TemporalReality.Present;
        } else { // is in present.. go to a different future according to water sources opened (requirements)
            originReality = TemporalReality.Present;


            int bestRequirementsCount = -1;
            TemporalReality bestRealityToTravel = TemporalReality.NoWaterFuture;

            foreach(TravelTimeRequirement req in TravelRequirements) {
                int totalClosed = 0;
                for(int i=0;i<req.WaterSourcesRequired.Count;i++) {
                    if(_waterSourcesClosed.ContainsKey(req.WaterSourcesRequired[i])){
                        if(_waterSourcesClosed[req.WaterSourcesRequired[i]]) {
                            totalClosed++;
                        }
                    }
                }

                if(totalClosed == req.WaterSourcesRequired.Count) { // this requirement was achieved
                    // check among all others requirement achieved. The one with more
                    // closed water sources count is the reality to travel to
                    if(totalClosed > bestRequirementsCount) {
                        bestRequirementsCount = totalClosed;
                        bestRealityToTravel = req.RealityToTravel;
                    }
                }
            }

            if(bestRequirementsCount != -1) {
                destinationReality = bestRealityToTravel;
            } else { // none of the requirements were satisfied, so, go to darkest future by default
                destinationReality = TemporalReality.NoWaterFuture;
            }
        }

        // ACTUAL TIME TRAVEL!
        SceneManager.LoadSceneAsync(destinationReality.ToString(), LoadSceneMode.Additive);
        SceneManager.UnloadScene(originReality.ToString());
    }



    // Registers and updates all WaterSources
    public void OnWaterSourceLoad(WaterSource source)
    {
        if(false == _waterSourcesClosed.ContainsKey(source.WaterSourceTag)) { // first loading
            _waterSourcesClosed.Add(source.WaterSourceTag, false); // all water sources are opened by default;
        }

        source.SwitchWaterFlow(_waterSourcesClosed[source.WaterSourceTag]);
    }
}
