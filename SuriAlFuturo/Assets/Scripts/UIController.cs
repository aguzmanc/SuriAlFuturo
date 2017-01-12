using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    public GameObject EndScreen;
    public GameObject TextFin;
    public GameObject ButtonGoToCredits;


    bool _isEnding;




    void Start() 
    {
        
    }



    void Update() 
    {
     
    }



    public void ShowEndScreen()
    {
        StartCoroutine(EndCredits());
    }


    IEnumerator EndCredits()
    {
        EndScreen.SetActive(true);
        TextFin.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        EndScreen.GetComponent<FadeInFadeOut>().FadeIn();

        yield return new WaitForSeconds(0.3f);

        TextFin.GetComponent<FadeInFadeOut>().FadeIn();

        yield return new WaitForSeconds(2f);

        ButtonGoToCredits.SetActive(true);
    }




}
