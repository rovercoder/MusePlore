using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveLoadFoundArtifacts {

    public void save(string category, string artifact) {

        string progress = PlayerPrefs.GetString("foundArtifacts");

        if (!progress.Contains(category + "_" + artifact))
        {
            if (progress == null || progress.Length == 0)
                PlayerPrefs.SetString("foundArtifacts", category + "_" + artifact);
            else
                PlayerPrefs.SetString("foundArtifacts", progress + "," + category + "_" + artifact);

            PlayerPrefs.Save();
        }

    }

    public List<List<string>> load()
    {
        string progress = PlayerPrefs.GetString("foundArtifacts");

        List<List<string>> foundArtifacts = new List<List<string>>();

        if (progress != null && progress.Length > 0)
        {
            string[] artifacts = progress.Split(',');
            foreach (string artifact in artifacts)
            {
                foundArtifacts.Add(new List<string>(artifact.Split('_')));
            }
        }

        return foundArtifacts;

    }

    public void pushArtifact(string category, string artifact)
    {
        PlayerPrefs.SetString("chosenArtifact", category + "_" + artifact);
        PlayerPrefs.Save();
    }

    public List<string> pullArtifact()
    {
        List<string> artifact = new List<string>(PlayerPrefs.GetString("chosenArtifact").Split('_'));
        PlayerPrefs.SetString("chosenArtifact", "");
        PlayerPrefs.Save();

        return artifact;
    }

    public Dictionary<string,System.Object> getArtifactData(string category, string artifact)
    {
        Dictionary<string, System.Object> artifactData = new Dictionary<string, System.Object>();

        string artifactLocation = "Artifacts/" + category + "/" + artifact;

        Texture2D texture = (Texture2D)Resources.Load(artifactLocation, typeof(Texture2D));
        artifactData.Add("image", (System.Object)texture);

        TextAsset text = (TextAsset)Resources.Load(artifactLocation, typeof(TextAsset));
        List<string> lines = new List<string>(text.text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
        artifactData.Add("text", (System.Object)lines);

        return artifactData;

    }

    public Dictionary<string, List<string>> getArtifactQuiz(string category)
    {
        Dictionary<string, List<string>> questionAnswers = new Dictionary<string, List<string>>();

        string quizLocation = "Artifacts/" + category + "/quiz";
        TextAsset text = (TextAsset)Resources.Load(quizLocation, typeof(TextAsset));
        List<string> lines = new List<string>(text.text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

        foreach(string line in lines)
        {
            String[] linesParsed = line.Split(new[] { "<>" }, StringSplitOptions.None);
            String question = linesParsed[0];

            List<string> answers = new List<string>();
            for (int i = 1; i < linesParsed.Length; i++) answers.Add(linesParsed[i]);

            questionAnswers.Add(question, answers);
        }

        return questionAnswers;

    }

}
