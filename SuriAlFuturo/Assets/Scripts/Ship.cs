using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour 
{
    public Renderer BodyRenderer;
    public float BlinkFrequency = 1f;

    Coroutine _blinkCoroutine;

	void Start () 
    {
        
	
	}
	
	void Update () {
	
	}


    /*  Put main characters on board */
    public void LoadCharacters()
    {
        StopBlinking();

        // more implementation needed
        throw new UnityException("LoadSuri not implemented");
    }


    /** Notices this ship is in range to be boarded **/
    public void StartBlinking() 
    {
        _blinkCoroutine = StartCoroutine(Blink());
    }


    /** Notices this ship is out of range to be boarded **/
    public void StopBlinking()
    {
        StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = null;
    }



    #region COROUTINES

    IEnumerator Blink()
    {
        while(true){
            yield return new WaitForSeconds(BlinkFrequency);
            BodyRenderer.material.color = Color.green;
            yield return new WaitForSeconds(BlinkFrequency);
            BodyRenderer.material.color = Color.white;
        }
    }

    #endregion
}
