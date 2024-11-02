using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum TestStatus
{
    Success,
    Fail,
    Now,
    None
}
public class TestMannger : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject TestGui;
    public Texture[] StatusImage;
    private List<Question> questions;
    private int successCount;
    // Start is called before the first frame update
    void Start()
    {
        successCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PopQuestions()
    {
        foreach (var question in questions)
        {
            question.gameObject = GameObject.Instantiate(Prefab, TestGui.transform);
            question.gameObject.GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = question.DisplayName;
            question.gameObject.GetComponentInChildren<RawImage>().texture = StatusImage[(int)question.status];
        }
    }

    public void LoadQuestionsList(List<string> questions)
    {
        string[] names;
        this.questions = new List<Question>();
        foreach (var question in questions)
        {
            names = (question.Contains(ValuePasser.Separator)) ? question.Split(ValuePasser.Separator) : new string[] { question, question };
            this.questions.Add(new Question(names[0], TestStatus.None, names[1]));
        }
        PopQuestions();
    }
    public string GetQuestions(int index)
    {
        var question = this.questions[index];
        question.status = TestStatus.Now;
        question.gameObject.GetComponentInChildren<RawImage>().texture = StatusImage[(int)question.status];
        return question.part;
    }
    public bool IsQuestionsList()
    {
        return !(questions is null);
    }
    
    public int IsFinish(int index)
    {
        if(index==questions.Count)
        {
            return this.successCount;
        }
        return -1;
    }

    public void ResultQuestions(int index, bool IsSuccess)
    {
        var question = questions[index];

        if (IsSuccess)
        {
            question.status = TestStatus.Success;
            successCount++;
        }
        else question.status = TestStatus.Fail;
        question.gameObject.GetComponentInChildren<RawImage>().texture = StatusImage[(int)question.status];
    }
}
public class Question
{
    public Question(string part, TestStatus status, string DisplayName)
    {
        this.part = part ?? throw new ArgumentNullException(nameof(part));
        this.DisplayName = DisplayName ?? throw new ArgumentNullException(nameof(DisplayName));
        this.status = status;

    }
    public GameObject gameObject { get; set; }
    public string part { get; set; }
    public string DisplayName { get; set; }
    public TestStatus status { get; set; }
}
