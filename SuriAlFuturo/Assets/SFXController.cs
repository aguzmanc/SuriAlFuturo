using UnityEngine;
using System.Collections;

public class SFXController : MonoBehaviour 
{
    public AudioSource EmbarkSound;
    public AudioSource DisembarkSound;
    public AudioSource PickItemSound;
    public AudioSource ButtonSound;
    public AudioSource ErrorSound;
    public AudioSource WaterOnSound;
    public AudioSource WaterOffSound;
    public AudioSource TimeTravelSound;
    public AudioSource TimeMachineOnSound;

    // Suri Dialogue
    public AudioSource SuriHappy;
    public AudioSource SuriSad;
    public AudioSource SuriCrying;
    public AudioSource SuriSurprised;



	void Start () {
	
	}
	
	void Update () {
	    
	}


    public void PlayEmbark(){EmbarkSound.Play();}

    public void PlayDisembark(){DisembarkSound.Play();}

    public void PlayPickItem(){PickItemSound.Play();}

    public void PlayButton(){ButtonSound.Play();}

    public void PlayError(){ErrorSound.Play();}

    public void PlayWaterOn(){WaterOnSound.Play();}

    public void PlayWaterOff(){WaterOffSound.Play();}

    public void PlayTimeTravel(){TimeTravelSound.Play();}

    public void PlayTimeMachineOn(){TimeMachineOnSound.Play();}

    public void PlayDialogue(string character, string emotion) 
    {
        if(character=="Suri"){
            if(emotion=="sad") SuriSad.Play();
            else if(emotion=="surprised") SuriSurprised.Play();
            else if(emotion=="happy") SuriHappy.Play();
            else if(emotion=="cry") SuriCrying.Play();
        }
    }

}
