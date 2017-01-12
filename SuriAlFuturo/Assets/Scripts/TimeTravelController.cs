using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

using SuriAlFuturo;

public class TimeTravelController : MonoBehaviour 
{
    bool _presentLoaded;
    bool _firstPresentLoad;
    bool _waitForTitle;

    public List<string> RealitySceneName;
    [HideInInspector]
    public string CurrentReality = "Present";

    public FadeInFadeOut Title;
    public FadeInFadeOut PressKeyToContinueText;
    public GameObject PressKeyToStartButton;
    public FadeInFadeOut UIFlash;


    void Awake()
    {
        _presentLoaded = false;
        _firstPresentLoad = true;
        _waitForTitle = true;

        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
        

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Present" && _firstPresentLoad){
            _firstPresentLoad = false;
            _waitForTitle = false;
        }

        UIFlash.FadeOut();
    }


    void Start ()
    {
        UIFlash.Show();

        // Unload Al Scenes except BaseScene  (just for debug in unity)
        for (int i=0; i<SceneManager.sceneCount; i++) {
            string sceneName = SceneManager.GetSceneAt(i).name;
            if(sceneName != "BaseScene"){
                SceneManager.UnloadScene(sceneName);
            }
        }

        StartCoroutine(PresentationSequence());
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


    public void ShowFlash()
    {
        UIFlash.Show();
    }


    public void StartGame() 
    {
        Title.gameObject.SetActive(false);
        PressKeyToContinueText.gameObject.SetActive(false);

        if(CurrentReality != "Present") {
            SceneManager.UnloadScene("Present");
            SceneManager.LoadSceneAsync(CurrentReality, LoadSceneMode.Additive);

            UIFlash.Show();
        }


    }



    IEnumerator PresentationSequence () 
    {
        //PressKeyToStartButton.SetActive(false);
        
        SceneManager.LoadSceneAsync("Present", LoadSceneMode.Additive);

        yield return new WaitWhile(()=>_waitForTitle);

        yield return new WaitForSeconds(3f);

        Title.FadeIn();

        yield return new WaitUntil(()=>Title.HasFinishedFade);

        PressKeyToContinueText.FadeIn();

        //yield return new WaitUntil(()=>PressKeyToContinueText.HasFinishedFade);

        //PressKeyToStartButton.SetActive(true);
    }
}
