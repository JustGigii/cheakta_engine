using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


enum music
{
    Test,
    Practis
}
public class UiMannger : MonoBehaviour
{

    public GameObject UIModel;
    public UiConroll uiConrole;
    public GameObject placeholder;
    public GameObject Panel;
    public AudioClip[] audioClips;
    public AudioSource Background;
    public loadjson jsondetails;
    
    [SerializeField][Range(5, 25)] int lenght;
  
    private OutlineAnimation manneger;
    private Intractable intractable;
    private GameObject currectshow;
    private TestMannger test;
    private List<string> notlearn;
    private int qestionNumber;
    private bool onqestion;

    GameObject targer;
    GameObject theCorect;
    // Start is called before the first frame update
    void Start()
    {
        //ValuePasser.plane = "F15";
        //ValuePasser.isTest = false;
        LoadComponent();
      
    }


    // Update is called once per frame
    void Update()
    {
        if(notlearn ==null&&jsondetails.plan != null) notlearn= jsondetails.plan.Study.Keys.ToList();
        
        if (uiConrole.isInMenu) return;
        if (jsondetails == null) return;
        if (ValuePasser.isTest)
        {
            TestLogic();
            return;
        }
        intractable.PlayerInteract();
        LoadModelDetails();
    }
    private void TestLogic()
    {
        uiConrole.mouse.SetActive(false);
        if (targer != null) { targer.GetComponent<cakeslice.Outline>().enabled = true; }
        PopTestDetail();
        ShowQestion();
        intractable.InteractSate(onqestion);
        if (intractable.highlight is null) return;
        string highlightName = intractable.highlight.name ?? "";
        if (manneger.playScanAnimtion) uiConrole.playsfx(sfx.scan);
        if (!manneger.playScanAnimtion && jsondetails.plan.Study.ContainsKey(highlightName)&& currectshow==null)
        {
            uiConrole.stopsfx();
            if (!Input.GetMouseButtonDown(0))
            {
                uiConrole.mouse.SetActive(true);
                return;
            }
            
            onqestion = false;
            targer = GameObject.Find(highlightName);
            EnableOutline(targer);

            if (ShowQestion().Contains(highlightName))
            {
                uiConrole.playsfx(sfx.currect);
                CreateInstant(targer);
            }
            else
            {
                theCorect = GameObject.Find(ShowQestion());
                EnableOutline(theCorect);
                targer.GetComponent<cakeslice.Outline>().color = 2;
                CreateInstant(theCorect);
                placeholder.SetActive(false);
                uiConrole.playsfx(sfx.Incurrect);
            }
            StartCoroutine(WaitSecends(3));
            

        }
    }
    private void EnableOutline(GameObject outlineoj)
    {
        if (outlineoj == null) return;
        if (outlineoj.GetComponent<cakeslice.Outline>() != null)
            outlineoj.GetComponent<cakeslice.Outline>().enabled = true;
        else outlineoj.AddComponent<cakeslice.Outline>();
        outlineoj.GetComponent<cakeslice.Outline>().color = 1;
    }

    private string ShowQestion()
    {
        string part = test.GetQuestions(this.qestionNumber);
        string title = (jsondetails.plan.Study[part].DisplayName is object) ? jsondetails.plan.Study[part].DisplayName : part;
        string Discripton = jsondetails.plan.Exam[part].Description;
        uiConrole.LoadPanelDetils(title, Discripton);
        return part;
    }

    private void LoadComponent()
    {
        jsondetails = GameObject.Find("json").GetComponent<loadjson>();
        manneger = this.GetComponent<OutlineAnimation>();
        intractable = this.GetComponent<Intractable>();
        test = this.GetComponent<TestMannger>();
        qestionNumber = 0;
        onqestion = true;
        targer = null;
        theCorect = null;
        GetComponent<AudioSource>().clip = (ValuePasser.isTest) ? audioClips[(int)music.Test] : audioClips[(int)music.Practis];
        GetComponent<AudioSource>().Play();
    }

    private void PopTestDetail()
    {
        if (!test.IsQuestionsList())
        {
            List<string> exam = jsondetails.plan.Exam.Keys.ToList();
            for (int i = 0; i < exam.Count; i++)
            {
                var name = jsondetails.plan.Exam[exam[i]].DisplayName;
                exam[i] = (name is object) ? name + ValuePasser.Separator + exam[i] : exam[i];
            }
            ValuePasser.Shuffle<string>(exam);
            test.LoadQuestionsList(exam);


        }
    }

    

    private void LoadModelDetails()
    {
        if (intractable.highlight != null)// cheack if to Model load
        {
            string highlightName = intractable.highlight.name;
            if (!jsondetails.plan.Study.ContainsKey(highlightName)) return;
            if (manneger.playScanAnimtion)
            {
                ScaningDetails();
            }
            else if (currectshow == null)
            {
                PanelDetail(highlightName);
                notlearn.Remove(highlightName);
                int all = jsondetails.plan.Study.Keys.Count;
                float succes = all - (float)notlearn.Count;
                StartCoroutine(uiConrole.PopCongar(Mathf.RoundToInt( succes/all * 100)));
            }
        }
        else
        {
            uiConrole.stopsfx();
            Panel.transform.LeanScale(Vector3.zero, 0.05f);
            Destroy(currectshow);
            currectshow = null; ;
        }
    }

    private void PanelDetail(string highlightName)
    {
        uiConrole.stopsfx();
        placeholder.SetActive(false);
        currectshow = CreateInstant(intractable.highlight);
        string title = jsondetails.plan.Study[highlightName].DisplayName == null ? highlightName : jsondetails.plan.Study[highlightName].DisplayName;
        string Discripton = jsondetails.plan.Study[highlightName].Description;
        uiConrole.LoadPanelDetils(title, Discripton);
    }

    private void ScaningDetails()
    {
        loadPanel();
        uiConrole.playsfx(sfx.scan);
        uiConrole.LoadPanelDetils("0x" + GenerateRandomString("ABCDEF123456789", 8), GenerateRandomWords());
        placeholder.SetActive(true);
        Destroy(currectshow);
        currectshow = null; ;
    }

    private void loadPanel()
    {
        if (Panel.transform.localScale == Vector3.zero)
            Panel.transform.LeanScale(Vector3.one, 0.2f);
    }

    private GameObject CreateInstant(GameObject insstance)
    {
        if (insstance == null) return null;
        currectshow = Instantiate(insstance);
        currectshow.layer = 12;
        currectshow.transform.position = UIModel.transform.position;
        currectshow.transform.Rotate(Vector3.zero);
        //normalizeassetx(currectshow);
        currectshow.AddComponent<cakeslice.Rotate>();
        return currectshow;
    }


    private string GenerateRandomString(string AllowedChars, int length)
    {
        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, AllowedChars.Length);
            sb.Append(AllowedChars[randomIndex]);
        }

        return sb.ToString();
    }

    private string GenerateRandomWords()
    {
        string discription = "";
        string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        for (int i = 0; i < lenght; i++)
        {
            discription += GenerateRandomString(AllowedChars, Random.Range(2, 5)) + " ";
        }
        return discription;
    }
    //public static void normalizeassetx(GameObject asset)
    //{
    //    var rend = asset.GetComponent<Renderer>();
    //    //Bounds bound = rend.bounds;
    //    //print(bound);
    //    //float ma;
    //    //float sizer = 10;
    //    //if (bound.size.x >= bound.size.y && bound.size.y >= bound.size.z) ma = bound.size.x;
    //    //else if (bound.size.y >= bound.size.z && bound.size.z >= bound.size.x) ma = bound.size.y;
    //    //else ma = bound.size.z;
    //    //ma = ma / sizer;
    //    //asset.transform.localScale = new Vector3(1 / ma, 1 / ma, 1 / ma);
    //    var MySize = rend.bounds.size / 10;
    //    var MaxSize = 1 / Mathf.Max(MySize.x, Mathf.Max(MySize.y, MySize.z));
    //    asset.transform.localScale = new Vector3(MaxSize, MaxSize, MaxSize);
    //}
    IEnumerator WaitSecends(int secends)
    {
        //Print the time of when the function is first called.
        targer.GetComponent<cakeslice.Outline>().enabled = true;
        bool result = theCorect == null;
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(secends);
        Destroy(this.currectshow);
        this.placeholder.active = true;
        ResetOutline(targer);
        ResetOutline(theCorect);
        currectshow = targer = theCorect = null;
        test.ResultQuestions(qestionNumber, result);
        qestionNumber++;
         int succes =test.IsFinish(qestionNumber);
        if(succes !=-1) {
            uiConrole.playsfx(sfx.job);
            uiConrole.loadTestContent(succes, jsondetails.plan.Exam.Keys.Count);
            ValuePasser.IsMove = false;
            ValuePasser.isTest =false;
        }
        //After we have waited 5 seconds print the time again.
        yield return new WaitForSeconds(0.5f);
        onqestion = true;


    }

    private void ResetOutline(GameObject outline)
    {
        if (outline != null)
        {
            outline.GetComponent<cakeslice.Outline>().color = 0;
            outline.GetComponent<cakeslice.Outline>().enabled = false;
        }
        Debug.Log("outline is null");
    }
}
