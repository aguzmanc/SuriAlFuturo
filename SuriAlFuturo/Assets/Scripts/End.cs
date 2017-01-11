using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class End : MonoBehaviour {
    public Talkable TheTalkable;
    public GameObject Suri;

    private UIController _uiController;

    void Start () 
    {
        Suri = GameObject.FindGameObjectWithTag(Tag.GameController).
            GetComponent<GameController>().Suri;

        _uiController = GameObject.FindGameObjectWithTag(Tag.GameController).GetComponent<UIController>();
    }



    void Update () 
    {
	    if (TheTalkable.WasRead) {
            TheTalkable.enabled = false;
            Suri.GetComponent<CharacterMovement>().IsControlledByPlayer = false;
            Suri.GetComponent<CharacterMovement>().enabled = false;

            _uiController.ShowEndScreen();
            this.enabled = false;
        }
    }
}
