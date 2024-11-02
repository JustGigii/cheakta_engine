using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Numerics;
using UnityEngine.SceneManagement;

public class loadjson : MonoBehaviour
{
    private string json;
    public Config config;
    public Planes plan;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        ValuePasser.loadModel = false;
        DontDestroyOnLoad(gameObject);

        loadcofig();
        //LoadDictionary();
        ValuePasser.plane = config.name;
        plan = GetPlanDetails();
        using (WWW web = new WWW(config.modelurl))
        {
            yield return web;
            AssetBundle remoteAssetBundle = web.assetBundle;
            if (remoteAssetBundle == null)
            {
                Debug.LogError("Failed to download AssetBundle!");
                yield break;
            }
            ValuePasser.loadModel = true;
            var plan = Instantiate(remoteAssetBundle.LoadAsset(config.name));
            DontDestroyOnLoad(plan);
            remoteAssetBundle.Unload(false);
            SceneManager.LoadScene("welcome", LoadSceneMode.Single);
        }
        

    }

    //private void Start()
    //{
    //    loadcofig();
    //    ValuePasser.plane = config.name;
    //    //LoadDictionary();
    //    plan = GetPlanDetails();
    //    ValuePasser.isTest= true;
    //}

    private Dictionary<string, partdetails> LoadDictionary(string part)
    {
        StartCoroutine(loadfile(config.url + ValuePasser.plane + part));
        var parts = new Dictionary<string, partdetails>();
        partdetails[] array = JsonConvert.DeserializeObject<partdetails[]>(json);
        foreach (var item in array)
        {
            if(!parts.ContainsKey(item.Name))
            {
                parts.Add(item.Name, item); 
            }
        }
        return parts;
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private void loadcofig()
    {
        string configtxt = System.IO.File.ReadAllText(UnityEngine.Application.dataPath+"/StreamingAssets/config.json", System.Text.Encoding.UTF8);
        config = JsonConvert.DeserializeObject<Config>(configtxt);
       


    }

    public Planes GetPlanDetails()
    {
        Planes plan = new Planes();
        plan.Exam = LoadDictionary("/Exam.json");
        plan.Study = LoadDictionary("/Part.json");

        return plan;
    }
    IEnumerator loadfile(string pathPath)
    {
        string file = "";
        if(pathPath.Contains("://"))
        {
            WWW www = new WWW(pathPath);
            yield return www;
            file = www.text;
        }
        else file = System.IO.File.ReadAllText(UnityEngine.Application.dataPath + pathPath, System.Text.Encoding.UTF8);
        this.json = file;

    }
    
}

public class Config
{
    public string url { get; set; }
    public string modelurl { get; set; }
    public string name { get; set; }
}

public class partdetails
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
}

public class Planes
{

    public Dictionary<string, partdetails> Study { get; set; }
    public Dictionary<string, partdetails> Exam { get; set; }
}