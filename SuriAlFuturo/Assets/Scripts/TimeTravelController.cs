using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

using SuriAlFuturo;

public class TimeTravelController : MonoBehaviour {
    public List<string> RealitySceneName;

    private TapController _tapController;
    private string _currentReality;

    void Start () {
        _tapController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<TapController>();

        // check if "Present" scene is in loaded scenes
        // (debug purposes in editor when testing)
        bool isLoaded = false;

        // Suri is in present time at beginning of the game
        _currentReality = RealitySceneName[0];

        for (int i=0; i<SceneManager.sceneCount; i++){
            string sceneName = SceneManager.GetSceneAt(i).name;
            if(sceneName.CompareTo(RealitySceneName[0]) == 0) {
                isLoaded = true;
                break;
            }
        }

        if(false == isLoaded) {
            SceneManager.LoadSceneAsync(_currentReality, LoadSceneMode.Additive);
        }
    }

    public void OnTimeTravel () {
        string lastReality = _currentReality;

        if (_currentReality != RealitySceneName[0]) {
            _currentReality = RealitySceneName[0];
        } else {
            _currentReality = RealitySceneName[GetFutureRealityIndex()];
        }
        SceneManager.LoadSceneAsync(_currentReality, LoadSceneMode.Additive);
        SceneManager.UnloadScene(lastReality);
    }

    public int GetFutureRealityIndex () {
        return _tapController.CountWaterTapsOff() + 1;
    }
}
