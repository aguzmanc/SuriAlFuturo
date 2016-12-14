using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitlePresentation : MonoBehaviour 
{
    public FadeInFadeOut Flash;
    public FadeInFadeOut MMAyALogo;
    public FadeInFadeOut MMayABg;
    public FadeInFadeOut AncestralGods;
    public FadeInFadeOut FlashToGame;

	void Start () 
    {
        StartCoroutine(Presentation());
	}
	
	void Update () 
    {
	
	}

    IEnumerator Presentation()
    {
        yield return new WaitForSeconds(2f);

        Flash.FadeOut();

        yield return new WaitUntil(()=>Flash.HasFinishedFade);

        yield return new WaitForSeconds(2f);

        MMAyALogo.FadeOut();
        MMayABg.FadeOut();

        yield return new WaitUntil(()=>MMAyALogo.HasFinishedFade);

        yield return new WaitForSeconds(2f);

        AncestralGods.FadeOut();

        yield return new WaitUntil(()=>AncestralGods.HasFinishedFade);

        yield return new WaitForSeconds(3f);

        FlashToGame.FadeIn();

        yield return new WaitUntil(()=>FlashToGame.HasFinishedFade);

        SceneManager.LoadScene("BaseScene");

    }
}
