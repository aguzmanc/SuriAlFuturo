using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class End : MonoBehaviour {
    public Talkable TheTalkable;
    public GameObject Suri;
    public GameObject Credits;

    void Start () {
        Suri = GameObject.FindGameObjectWithTag(Tag.GameController).
            GetComponent<GameController>().Suri;
        Credits = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<UIController>().Credits;
    }
    
    void Update () {
	if (TheTalkable.WasRead) {
            TheTalkable.enabled = false;
            Suri.GetComponent<CharacterMovement>().IsControlledByPlayer = false;
            Suri.GetComponent<CharacterMovement>().enabled = false;
            Credits.SetActive(true);
            this.enabled = false;
        }
    }
}
