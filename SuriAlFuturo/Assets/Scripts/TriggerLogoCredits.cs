using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class TriggerLogoCredits : MonoBehaviour 
{
    public RectTransform Trigger;
    public FadeInFadeOut WWW;
    public GameObject ButtonReset;
    bool _waiting;


	void Start () 
    {
        _waiting = true;
	}
	


	void Update () 
    {
        if(Trigger.position.y > 400 && _waiting) {
            

            _waiting = false;

            StartCoroutine(Reset());
        }
	}

    IEnumerator Reset()
    {
        GetComponent<FadeInFadeOut>().FadeIn();
        WWW.FadeIn();

        yield return new WaitForSeconds(10f);

        ButtonReset.SetActive(true);
    }


    public void ResetGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        File.Delete(Application.persistentDataPath + "/" + PersistenceController.FileName);

        Application.OpenURL("http://www.ancestralgods.games");

        SceneManager.LoadScene("BaseScene");
    }
}
