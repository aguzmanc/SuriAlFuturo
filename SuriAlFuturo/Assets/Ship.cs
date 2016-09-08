using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour 
{

	void Start () {
	
	}
	
	void Update () {
	
	}


    /*  Put main characters on board */
    public void LoadCharacters()
    {
        throw new UnityException("LoadSuri not implemented");
    }


    /** Notices this ship is in range to be boarded **/
    public void StartBlinking() 
    {
        throw new UnityException("StartBlinking not implemented");
    }


    /** Notices this ship is out of range to be boarded **/
    public void StopBlinking()
    {
        throw new UnityException("StopBlinking not implemented");
    }
}
