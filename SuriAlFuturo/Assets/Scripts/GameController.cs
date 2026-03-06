using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
{
    public static GameController _instance;

    public static int TOTAL_HEALTH = 5;

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

    private int _health;
    private bool _gameFinished;
    

    public static bool gameFinished
    {
        get
        {
            if(_instance == null) return true;

            return _instance._gameFinished;
        }
    }

    public static event System.Action onDamage;
    public static event System.Action onGameFinished;



    void Awake()
    {
        _instance = this;    
    }


    void Start () {
        _collectionSystem = GetComponent<CollectionSystem>();
        _dialogueController = GetComponent<DialogueController>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        FollowingCamera.gameObject.SetActive(false);

        _health = TOTAL_HEALTH;
        _gameFinished = false;
    }


    void Update () {
        if (Input.GetButtonDown("Start")) {
            GetComponent<TimeTravelController>().StartGame();
            StartGame();
            StartButton.SetActive(false);
        }
    }


    void OnDestroy()
    {
        _instance = null;
    }


    public static void Damage()
    { 
        onDamage ? .Invoke();
        _instance._health --;

        if(_instance._health == 0) {
            onGameFinished ? . Invoke();
            _instance._gameFinished = true;
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



    public static void RestartGame()
    { 
        SceneManager.LoadScene("BaseScene", LoadSceneMode.Single);
    }

}
