using UnityEngine;
using System.Collections;

public class UnloadDockTrigger : MonoBehaviour 
{
    public Renderer BodyRenderer;
    public float BlinkFrequency = 1f;

    Coroutine _blinkCoroutine;
    bool _isShipInRange;
    Ship _currentShip;
    Color _originalAvatarColor;
	
	void Start () 
    {
        _isShipInRange = false;

        _originalAvatarColor = BodyRenderer.material.color;
        BodyRenderer.material.color = Color.clear;
	}
	
	
	void Update () 
    {
        if(_isShipInRange && Input.GetButtonDown("LoadUnloadToShip")) {
            StopBlinking();
            _currentShip.gameObject.GetComponent<CharacterMovement>().IsControlledByPlayer = false;

            GameObject suri = Suri.Instance;
            suri.transform.position =  BodyRenderer.gameObject.transform.position;
            suri.SetActive(true);
            suri.GetComponent<CharacterMovement>().IsControlledByPlayer = true;

            FollowCamera.Instance.Target = suri;
        }
	}



    void OnTriggerEnter(Collider coll)
    {
        Ship ship = coll.GetComponent<ShipUnloadReceiver>().Ship;

        if(false == ship.GetComponent<CharacterMovement>().IsControlledByPlayer)
            return;

        StartBlinking();
        _isShipInRange = true;
        _currentShip = ship;
    }

        
    void OnTriggerExit(Collider coll)
    {
        StopBlinking();
        _isShipInRange = false;
        _currentShip = null;
    }


    /** Ship is in range to be boarded **/
    private void StartBlinking() 
    {
        _blinkCoroutine = StartCoroutine(Blink());
    }


    /** Ship is out of range to be boarded or has unloaded **/
    private void StopBlinking()
    {
        StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = null;
        BodyRenderer.material.color = Color.clear;
    }





    #region COROUTINES

    IEnumerator Blink()
    {
        while(true){
            yield return new WaitForSeconds(BlinkFrequency);
            BodyRenderer.material.color = _originalAvatarColor;
            yield return new WaitForSeconds(BlinkFrequency);
            BodyRenderer.material.color = Color.clear;
        }
    }

    #endregion
}
