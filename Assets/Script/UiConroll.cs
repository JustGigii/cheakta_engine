using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum sfx{
    scan,
    select,
    currect,
    Incurrect,
    job
}
public class UiConroll : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource sfxmannger;
    public RTLTMPro.RTLTextMeshPro title;
    public RTLTMPro.RTLTextMeshPro Discripton;
    public RTLTMPro.RTLTextMeshPro all;
    public RTLTMPro.RTLTextMeshPro Succses;
    public RTLTMPro.RTLTextMeshPro Grade;
    public RTLTMPro.RTLTextMeshPro prest;
    public bool isInMenu;
    public GameObject Menuetest;
    public GameObject menuPanel;
    public GameObject mouse;
    public GameObject congart;
    private Transform box;
    public GameObject testpanel;
    public float menuYpos = 50f;
    // Start is called before the first frame update
    void Start()
    {
        box = GameObject.Find("Menu").transform;
        box.localPosition = new Vector2(0, -Screen.height);
        menuPanel.GetComponent<CanvasGroup>().alpha = 0;
        mouse.SetActive(false);
        congart.transform.localPosition= new Vector2(0, -Screen.height);
        this.Menuetest.SetActive(false);
        testpanel.SetActive(ValuePasser.isTest);
        isInMenu = false;
    }

    
    // Update is called once per frame
    void Update()
    {
        testpanel.SetActive(ValuePasser.isTest);
        Menuloader();
    }
    private void Menuloader()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {

            if (menuPanel.GetComponent<CanvasGroup>().alpha == 0)//if menuclose
            {
                isInMenu = true;
                menuPanel.GetComponent<CanvasGroup>().LeanAlpha(1, 0.5f);
                box.LeanMoveLocalY(menuYpos, 0.5f).setEaseOutExpo().delay = 0.1f;
            }
            else
            {
                isInMenu = false;
                menuPanel.GetComponent<CanvasGroup>().LeanAlpha(0, 0.5f);
                box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseOutExpo();
            }
        }
    }
    public void CloseMenu()
        {

        StartCoroutine(CloseMenuwithsound(0.5f));

    }
    public void loadwelcomeseance()
    {
        playsfx(sfx.select);
        SceneManager.LoadScene("welcome", LoadSceneMode.Single);
    }
    public void LoadTest()
    {
        playsfx(sfx.select);
        ValuePasser.isTest = true;
        SceneManager.LoadScene("F15", LoadSceneMode.Single);
    }
    public void loadTestContent(int succses,int all)
    {
        this.Menuetest.SetActive(true);
        menuPanel.GetComponent<CanvasGroup>().LeanAlpha(1, 0.5f);
        box.LeanMoveLocalY(menuYpos, 0.5f).setEaseOutExpo().delay = 0.1f;
        this.Succses.text = succses.ToString();
        this.all.text = all.ToString();
        int garde = Mathf.RoundToInt(succses / (float)all * 100);
        this.Grade.text = garde.ToString();
        if(garde>=80)
            this.Grade.color= Color.green;
        else
            this.Grade.color=Color.red;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isInMenu= true;

    }
    public IEnumerator PopCongar(int prestnum)
    {
        if (prestnum % 10 == 0)
        {
            congart.transform.LeanMoveLocalY(-359, 1f).setEaseOutExpo().delay = 0.1f;
            prest.text = prestnum.ToString()+"%";
            yield return new WaitForSeconds(5);
            congart.transform.LeanMoveLocalY(-Screen.height, 1.5f).setEaseOutExpo();
        }
        
    }
    public void LoadPanelDetils(string title, string Discripton)
    {
        
        this.title.text = title;
        this.Discripton.text = Discripton;
    }
    public void playsfx(sfx sound)
    {
        if (sfxmannger.isPlaying) return;
        sfxmannger.clip= audioClips[(int)sound];
        sfxmannger.Play();

    }
    public void stopsfx()
    {
        sfxmannger.Stop();
    }
    IEnumerator CloseMenuwithsound(float sec)
    {
        playsfx(sfx.select);
        yield return new WaitForSeconds(sec);
        menuPanel.GetComponent<CanvasGroup>().LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseOutExpo();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInMenu = false;
        ValuePasser.IsMove = true;
    }
}
