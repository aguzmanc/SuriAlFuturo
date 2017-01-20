using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public FollowCamera FollowingCamera;
    public GameObject TitleCamera;
    public GameObject Suri;
    public GameObject Chapu;
    public GameObject MovementGizmos;
    public GameObject ControlledCharacter;
    public GameObject StartButton;


    public bool CloseToBlocker;
    public bool CanTalk;
    public bool CanGive {
        get {
            return (_collectionSystem.CountOwnedItems() > 0) && CanTalk
                && false == _dialogueController.IsTalkingToSomeone()
                && CloseToBlocker;
        }
    }
    // tap == pila
    public bool CanUseTap;
    public List<GameObject> EnableOnStartGame;

    private bool _canGive;
    private CollectionSystem _collectionSystem;
    private DialogueController _dialogueController;

    void Start () {
        _collectionSystem = GetComponent<CollectionSystem>();
        _dialogueController = GetComponent<DialogueController>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        FollowingCamera.gameObject.SetActive(false);
    }

    void Update () {
        if (Input.GetButtonDown("Start")) {
            GetComponent<TimeTravelController>().StartGame();
            StartGame();
            StartButton.SetActive(false);
        }
    }

    public void SetDrivingBoat (bool value) {
        if (value) {
            FollowingCamera.EnterWater();
        } else {
            FollowingCamera.ExitWater();
        }
    }


    public void StartGame() 
    {
        TitleCamera.SetActive(false);
        FollowingCamera.gameObject.SetActive(true);

        foreach (GameObject g in EnableOnStartGame) {
            g.SetActive(true);
        }
    }


    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

}
