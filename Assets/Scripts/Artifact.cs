using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class Artifact : MonoBehaviour {

    SaveLoadFoundArtifacts saveloader = new SaveLoadFoundArtifacts();
    List<string> artifact = new List<string>();
    Texture2D image = null;

    // Use this for initialization
    void Start () {

        init();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void init()
    {
        artifact = saveloader.pullArtifact();
        Dictionary<string, System.Object> artifactData = saveloader.getArtifactData(artifact[0], artifact[1]);

        System.Object obj;
        artifactData.TryGetValue("image", out obj);
        image = (Texture2D)obj;
        
        if (image != null)
        {
            Image artifactImage = GameObject.Find("Image").GetComponent<Image>();
            artifactImage.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
        }

        artifactData.TryGetValue("text", out obj);
        List<string> text = (List<string>)obj;
        setTitleDescription(text[0], text[1]);

        showArtifactProps(true);

    }

    public void setTitleDescription(string title, string description)
    {
        Text artifactTitle = GameObject.Find("Title").GetComponent<Text>();
        Text artifactDescription = GameObject.Find("Description").GetComponent<Text>();

        artifactTitle.text = title;
        artifactDescription.text = description;
    }

    public Texture2D loadImageResource(string path, string filename)
    {
        Texture2D texture = null;

        System.Object[] textures = Resources.LoadAll(path, typeof(Texture2D));

        for (int i = 0; i < textures.Length; i++)
        {
            texture = (Texture2D)textures[i];
            if (texture.name.Equals(filename))
                return texture;
            
        }

        return null;

    }

    public void showArtifactProps(bool show)
    {
        Image closeButton = GameObject.Find("Close Button").GetComponent<Image>();
        Image artifactImage = GameObject.Find("Image").GetComponent<Image>();
        Text artifactTitle = GameObject.Find("Title").GetComponent<Text>();
        Text artifactDescription = GameObject.Find("Description").GetComponent<Text>();
        Image videoButton = GameObject.Find("Video Button").GetComponent<Image>();
        Image audioButton = GameObject.Find("Audio Button").GetComponent<Image>();

        closeButton.enabled = show;
        artifactImage.enabled = show;
        artifactTitle.enabled = show;
        artifactDescription.enabled = show;
        videoButton.enabled = show;
        audioButton.enabled = show;
    }

    public void playVideo()
    {
        var regex = new Regex(Regex.Escape(artifact[0][0].ToString()));
        Handheld.PlayFullScreenMovie("Artifacts/" + regex.Replace(artifact[0], artifact[0][0].ToString().ToUpper(), 1) + "/" + artifact[1] + ".mp4");
    }

    public void closeButton()
    {
        Application.LoadLevel("collections");
    }


}
