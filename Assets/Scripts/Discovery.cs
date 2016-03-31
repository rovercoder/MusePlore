using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;

public class Discovery : MonoBehaviour {

    SaveLoadFoundArtifacts saveLoader = new SaveLoadFoundArtifacts();
    int propertyPanelState = 1;

    // Use this for initialization
    void Start () {

        init();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void init()
    {
        PlayerPrefs.SetInt("VideoVisible", 0);
        PlayerPrefs.SetString("ImageTarget", "");
        showArtifactProps(false);
    }

    public void showArtifactProps(bool show)
    {
        if (propertyPanelState == 1)
        {
            openPropertyPanel(show);
            openMinimisedPropertyPanel(false);
        }
        else
        {
            openPropertyPanel(false);
            openMinimisedPropertyPanel(show);
        }

    }

    public void openArtifactProps(bool open)
    {
        if (open)
        {
            propertyPanelState = 1;
            openPropertyPanel(true);
            openMinimisedPropertyPanel(false);
        }
        else
        {
            propertyPanelState = 0;
            openPropertyPanel(false);
            openMinimisedPropertyPanel(true);
        }
    }

    public void openPropertyPanel(bool open)
    {
        Text artifactTitle = GameObject.Find("Property Panel").transform.Find("Title").GetComponent<Text>();
        Text artifactDescription = GameObject.Find("Property Panel").transform.Find("Description").GetComponent<Text>();
        Image videoButton = GameObject.Find("Property Panel").transform.Find("Video Button").GetComponent<Image>();
        Image hideButton = GameObject.Find("Property Panel").transform.Find("Hide Button").GetComponent<Image>();
        Image propertyPanel = GameObject.Find("Property Panel").GetComponent<Image>();

        artifactTitle.enabled = open;
        artifactDescription.enabled = open;
        videoButton.enabled = open;
        hideButton.enabled = open;
        propertyPanel.enabled = open;        
    }

    public void openMinimisedPropertyPanel(bool open)
    {
        Text artifactTitleMinimised = GameObject.Find("Hidden Panel").transform.Find("Title").GetComponent<Text>();
        Image showButton = GameObject.Find("Hidden Panel").transform.Find("Show Button").GetComponent<Image>();
        Image hiddenPanel = GameObject.Find("Hidden Panel").GetComponent<Image>();

        artifactTitleMinimised.enabled = open;
        showButton.enabled = open;
        hiddenPanel.enabled = open;
    }

    public void loadArtifactResources(string category, string artifact)
    {
        /*var regex = new Regex(Regex.Escape(category[0].ToString()));
        string categorySentenceCase = regex.Replace(category, category[0].ToString().ToUpper(), 1);
        Object[] artifactFiles = Resources.LoadAll("Artifacts/" + categorySentenceCase);
        foreach (Object file in artifactFiles)
        {
            /*
            if (file.Name.Equals(artifact))
            {
                file;
            }
            
            Debug.Log(file.name);
        }*/

        saveArtifactProgress(category, artifact);
    
        Dictionary<string, System.Object> artifactData = saveLoader.getArtifactData(category, artifact);
        
        System.Object obj = null;
        artifactData.TryGetValue("text", out obj);
        List<string> text = (List<string>)obj;
        setTitleDescription(text[0],text[1]);
        
        showArtifactProps(true);

        if (PlayerPrefs.GetInt("VideoVisible") == 0)
        {
            PlayerPrefs.SetInt("VideoVisible", 1);
            showhideVideos();
        }
    }

    public void setTitleDescription(string title, string description)
    {
        Text artifactTitle = GameObject.Find("Property Panel").transform.Find("Title").GetComponent<Text>();
        Text artifactTitleHidden = GameObject.Find("Hidden Panel").transform.Find("Title").GetComponent<Text>();
        Text artifactDescription = GameObject.Find("Property Panel").transform.Find("Description").GetComponent<Text>();

        artifactTitle.text = title;
        artifactTitleHidden.text = title;
        artifactDescription.text = description;
        
    }

    public void showhideVideos()
    {
        string imageTarget = PlayerPrefs.GetString("ImageTarget");
        if (imageTarget.Equals(""))
        {
            Debug.Log("Empty ImageTarget!");
            return;
        }

        if (PlayerPrefs.GetInt("VideoVisible") == 0) PlayerPrefs.SetInt("VideoVisible", 1);
        else
        {
            PlayerPrefs.SetInt("VideoVisible", 0);
        }

        GameObject video = GameObject.Find(imageTarget + "/Video").gameObject;
        GameObject icon = GameObject.Find(imageTarget + "/Video/Icon").gameObject;
        
        video.GetComponent<Renderer>().enabled = (PlayerPrefs.GetInt("VideoVisible") == 1);
        video.GetComponent<Collider>().enabled = (PlayerPrefs.GetInt("VideoVisible") == 1);
        icon.GetComponent<Renderer>().enabled = (PlayerPrefs.GetInt("VideoVisible") == 1);
        icon.GetComponent<Collider>().enabled = (PlayerPrefs.GetInt("VideoVisible") == 1);    
    }

    public void saveArtifactProgress(string category, string artifact)
    {
        saveLoader.save(category, artifact);
    }

    public void maximiseArtifactProps()
    {
        openArtifactProps(true);
    }

    public void minimiseArtifactProps()
    {
        openArtifactProps(false);
    }

    public void returnToMenu()
    {
        Application.LoadLevel("menu");
    }

}
