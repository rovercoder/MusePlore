using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Quiz : MonoBehaviour {

    SaveLoadFoundArtifacts saveloader = new SaveLoadFoundArtifacts();
    string category;
    Dictionary<string, List<string>> artifactQuiz;
    int questionNo = -1;
    string correctAnswer;
    int correctCount = 0;

    void Start () {
        init();
	}
	
	void Update () {
	
	}

    public void init()
    {
        GameObject.Find("Achievement").GetComponent<Image>().enabled = false;
        category = saveloader.pullArtifact()[0];
        artifactQuiz = saveloader.getArtifactQuiz(category);
        nextQuestion();
    }

    public void nextQuestion()
    {
        questionNo++;
        
        if (questionNo >= artifactQuiz.Keys.Count)
        {
            quizFinished();
        }
        else
        {
            for (int i = 1; i < 5; i++)
            {
                Image answerButton = GameObject.Find("Answer " + i).GetComponent<Image>();
                answerButton.enabled = false;
                answerButton.color = Color.white;
                answerButton.transform.Find("Text").GetComponent<Text>().enabled = false;
            }

            List<string> getKeys = new List<string>(artifactQuiz.Keys);
            List<string> answers = new List<string>();

            artifactQuiz.TryGetValue(getKeys[questionNo], out answers);

            GameObject.Find("Category").GetComponent<Text>().text = new Regex(Regex.Escape(category[0].ToString())).Replace(category, category[0].ToString().ToUpper(), 1);
            GameObject.Find("Question Indicator").GetComponent<Text>().text = "Question " + (questionNo + 1) + " of " + getKeys.Count;
            GameObject.Find("Question").GetComponent<Text>().text = getKeys[questionNo];

            for (int i = 0; i < answers.Count; i++)
                if (answers[i].EndsWith("#"))
                {
                    correctAnswer = answers[i].Substring(0, answers[i].Length - 1);
                    answers[i] = correctAnswer;
                    break;
                }

            for (int i = 0; i < answers.Count; i++)
            {
                Image answerButton = GameObject.Find("Answer " + (i + 1)).GetComponent<Image>();
                answerButton.enabled = true;

                Text answerText = answerButton.transform.Find("Text").GetComponent<Text>();
                answerText.text = answers[i];
                answerText.enabled = true;
            }

        }

    }

    public void guessAnswer(int answerIndex)
    {
        List<string> getKeys = new List<string>(artifactQuiz.Keys);
        List<string> answers = new List<string>();

        artifactQuiz.TryGetValue(getKeys[questionNo], out answers);

        int correctAnswerIndex = answers.IndexOf(correctAnswer) + 1;

        if (answerIndex == correctAnswerIndex) //CORRECT
        {
            correctCount++;
            GameObject.Find("Answer " + correctAnswerIndex).GetComponent<Image>().color = Color.green;
        }
        else GameObject.Find("Answer " + correctAnswerIndex).GetComponent<Image>().color = Color.yellow;

        for(int i = 1; i < answers.Count+1; i++) //INCORRECT
            if (i != correctAnswerIndex)
                GameObject.Find("Answer " + i).GetComponent<Image>().color = Color.red;

        StartCoroutine(delayNextQuestion());
    }

    IEnumerator delayNextQuestion()
    {
        yield return new WaitForSeconds(2);
        nextQuestion();
    }

    public void quizFinished()
    {
        int totalNoQuestions = new List<string>(artifactQuiz.Keys).Count;
        GameObject.Find("Category").GetComponent<Text>().text = "Quiz Complete!";
        GameObject.Find("Question Indicator").GetComponent<Text>().text = "";
        GameObject.Find("Question").GetComponent<Text>().text = "";
        GameObject.Find("Final Mark").GetComponent<Text>().text = "Score: " + correctCount + "/" + totalNoQuestions;

        for (int i = 1; i < 5; i++)
        {
            Image answerButton = GameObject.Find("Answer " + i).GetComponent<Image>();
            answerButton.enabled = false;
            answerButton.color = Color.white;
            answerButton.transform.Find("Text").GetComponent<Text>().enabled = false;
        }

        if (correctCount == totalNoQuestions)
            GameObject.Find("Achievement").GetComponent<Image>().enabled = true;

    }

    public void backToCollections()
    {
        Application.LoadLevel("collections");
    }

}
