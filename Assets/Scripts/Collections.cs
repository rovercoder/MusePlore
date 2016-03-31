using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Collections : MonoBehaviour {

    ArrayList categories = new ArrayList();
    Dictionary<string, int> pager = new Dictionary<string, int>();
    Dictionary<string, string> categoryImagePaths = new Dictionary<string, string>();
    Dictionary<string, Texture2D[]> categoryImages = new Dictionary<string, Texture2D[]>();
    Dictionary<string, string> categoryPanels = new Dictionary<string, string>();
    Dictionary<string, bool> categoryQuizUnlocked = new Dictionary<string, bool>();
    SaveLoadFoundArtifacts saveloader = new SaveLoadFoundArtifacts();
    List<List<string>> foundArtifacts = null;


    // Use this for initialization
    void Start () {

        init();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void init()
    {
        foundArtifacts = saveloader.load();

        categories.Add("mammals");
        categories.Add("birds");
        categories.Add("skeletons");

        string categorySentenceCase = "";

        foreach (string category in categories)
        {
            var regex = new Regex(Regex.Escape(category[0].ToString()));
            categorySentenceCase = regex.Replace(category, category[0].ToString().ToUpper(), 1);
            categoryImagePaths.Add(category, "Artifacts/" + categorySentenceCase);
            categoryImages.Add(category, loadImageResources(category));
            pager.Add(category, 0);
            categoryPanels.Add(category, categorySentenceCase + "Panel");

            Texture2D[] images = null;
            categoryImages.TryGetValue(category, out images);

            string categoryPanel = "";
            categoryPanels.TryGetValue(category, out categoryPanel);
            Text progress = GameObject.Find(categoryPanel).transform.Find("Progress").GetComponent<Text>();

            Image quizbtn = GameObject.Find(categoryPanel).transform.Find("Quiz Button").GetComponent<Image>();

            int foundArtifactsCount = checkFoundArtifacts(category).Count;

            if (foundArtifactsCount == images.Length)
            {
                categoryQuizUnlocked.Add(category, true);
                progress.enabled = false; //hide progress text (0/6)
                quizbtn.sprite = loadSprite("quiz"); //replace quiz button with a "QUIZ" button

            } else
            {
                progress.enabled = true;
                progress.text = foundArtifactsCount + "/"+ images.Length;
                quizbtn.sprite = loadSprite("padlock_unlocked");
            }

            loadThumbnails(category, true);
        }
    }

    public void loadNextMammalsThumbnails()
    {
        loadThumbnails("mammals", true);
    }

    public void loadPrevMammalsThumbnails()
    {
        loadThumbnails("mammals", false);
    }

    public void loadNextBirdsThumbnails()
    {
        loadThumbnails("birds", true);
    }

    public void loadPrevBirdsThumbnails()
    {
        loadThumbnails("birds", false);
    }

    public void loadNextSkeletonsThumbnails()
    {
        loadThumbnails("skeletons", true);
    }

    public void loadPrevSkeletonsThumbnails()
    {
        loadThumbnails("skeletons", false);
    }

    public void loadThumbnails(string category, bool next)
    {
        int page = 0;

        Texture2D[] images = null;
        categoryImages.TryGetValue(category, out images);

        pager.TryGetValue(category, out page);

        if (next && (page != images.Length / 3)) page++;
        else if (page != 1) page--; else page++;

        pager.Remove(category);
        pager.Add(category, page);
        
        int index = (page * 3) - 3;

        string categoryPanel = "";
        categoryPanels.TryGetValue(category, out categoryPanel);

        Text pageIndicator = GameObject.Find(categoryPanel).transform.Find("Page Indicator").GetComponent<Text>();
        pageIndicator.text = page+" of "+images.Length/3;

        int counter = 1;

        for (int i = index; i < index + 3; i++)
        {
            Image imagebtn = GameObject.Find(categoryPanel).transform.Find("Image" + counter).GetComponent<Image>();
            imagebtn.sprite = Sprite.Create(images[i], new Rect(0, 0, images[i].width, images[i].height), Vector2.zero);

            Image imagelock = imagebtn.transform.Find("Image" + counter + "Lock").GetComponent<Image>();

            if (checkFoundArtifacts(category).Contains(images[i].name))
                imagelock.enabled = false;
            else imagelock.enabled = true;
            
            counter++;
        }


    }

    public Texture2D[] loadImageResources(string category)
    {
        Texture2D[] typedTextures = null;
        Object[] textures = null;

        string path = "";
        categoryImagePaths.TryGetValue(category, out path);

        textures = Resources.LoadAll(path, typeof(Texture2D));

        typedTextures = new Texture2D[textures.Length];
        for (int i = 0; i < textures.Length; i++)
        {
            typedTextures[i] = (Texture2D)textures[i];
        }

        Debug.Log("Textures Loaded: " + typedTextures.Length);

        return typedTextures;

    }
    
    public Sprite loadSprite(string name)
    {
        return Resources.Load<Sprite>(name);
    }

    public List<string> checkFoundArtifacts(string category)
    {
        List<string> found = new List<string>();

        foreach (List<string> foundArtifact in foundArtifacts)
            if (foundArtifact[0].Equals(category))
                found.Add(foundArtifact[1]);

        return found;
    }

    public bool checkQuizUnlocked(string category)
    {
        bool unlocked = false;
        categoryQuizUnlocked.TryGetValue(category, out unlocked);
        return unlocked;
    }


    public void runMammalsQuiz()
    {
        runQuiz("mammals");
    }

    public void runBirdsQuiz()
    {
        runQuiz("birds");
    }

    public void runSkeletonsQuiz()
    {
        runQuiz("skeletons");
    }

    public void runQuiz(string category)
    {
        if (checkQuizUnlocked(category))
        {
            saveloader.pushArtifact(category, "");
            Application.LoadLevel("quiz");
        }
            
    }

    public void artifactClicked(string category, int imageNum)
    {
        Texture2D[] images = null;
        categoryImages.TryGetValue(category, out images);

        int page = 0;
        pager.TryGetValue(category, out page);

        if (checkFoundArtifacts(category).Contains(images[(page * 3) - 3 + imageNum - 1].name))
        {
            saveloader.pushArtifact(category, images[(page * 3) - 3 + imageNum - 1].name);
            Application.LoadLevel("artifact");
        }
    }

    public void image1Clicked(string category)
    {
        artifactClicked(category, 1);
    }

    public void image2Clicked(string category)
    {
        artifactClicked(category, 2);
    }

    public void image3Clicked(string category)
    {
        artifactClicked(category, 3);
    }

    public void returnToMenu()
    {
        Application.LoadLevel("menu");
    }


}
