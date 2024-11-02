using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SenceMangger : MonoBehaviour
{
    public PlayableDirector dir;
    public GameObject fade;
    // Start is called before the first frame update
    void Start()
    {
        ValuePasser.plane = "F15";
        if (fade is object) fade.SetActive(false);
        if (gameObject.name == "CutscenesMangger") StartCoroutine(Waitforcutscemeend());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Waitforload(float sec) 
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene("catsinstoAirPlane", LoadSceneMode.Single);
    }
    IEnumerator Waitforcutscemeend()
    {
        yield return new WaitForSeconds(29.1f);
        SceneManager.LoadScene("F15", LoadSceneMode.Single);
    }

    public void loadTest(bool istest)
    {
        fade.SetActive(true);
        ValuePasser.isTest = istest;
        dir.Play();
        StartCoroutine(Waitforload(0.5f));

    }
}
