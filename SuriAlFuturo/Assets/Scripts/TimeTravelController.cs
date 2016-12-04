using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

using SuriAlFuturo;

public class TimeTravelController : MonoBehaviour {
    public List<string> RealitySceneName;

    [HideInInspector]
    public string CurrentReality = "Present";

    void Start () 
    {
        // Unload Al Scenes except BaseScene
        for (int i=0; i<SceneManager.sceneCount; i++) {
            string sceneName = SceneManager.GetSceneAt(i).name;
            if(sceneName != "BaseScene"){
                SceneManager.UnloadScene(sceneName);
            }
        }

        // Load Current Reality Scene after loading all other ones
        SceneManager.LoadSceneAsync(CurrentReality, LoadSceneMode.Additive);
    }



    public void OnTimeTravel () 
    {
        string lastReality = CurrentReality;

        // going present, or going to future according to water taps 
        CurrentReality = (CurrentReality != "Present") ? "Present" : RealitySceneName[GetFutureRealityIndex()];

        SceneManager.LoadSceneAsync(CurrentReality, LoadSceneMode.Additive);
        SceneManager.UnloadScene(lastReality);

        PersistenceController.SaveDataToDisk();
    }



    public int GetFutureRealityIndex () 
    {
        TapController tapController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<TapController>();

        return tapController.CountWaterTapsOff() + 1;
    }
}
