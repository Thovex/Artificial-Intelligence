using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameDatabase : MonoBehaviour {

    public TextAsset firsts;
    private string firstsAsString;
    private List<string> firstsLines;

    public TextAsset seconds;
    private string secondsAsString;
    private List<string> secondsLines;

    public TextAsset thirds;
    private string thirdsAsString;
    private List<string> thirdsLines;

    private static NameDatabase instance = null;
    public static NameDatabase Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    private void Start() {
        firstsAsString = firsts.text;
        secondsAsString = seconds.text;
        thirdsAsString = thirds.text;

        firstsLines = new List<string>();
        firstsLines.AddRange(firstsAsString.Split("\n"[0]));

        secondsLines = new List<string>();
        secondsLines.AddRange(secondsAsString.Split("\n"[0]));

        thirdsLines = new List<string>();
        thirdsLines.AddRange(thirdsAsString.Split("\n"[0]));
    }


    public string GenerateName() { 
        var name = "";

        if (Random.value < .1) {
                name = firstsLines[Random.Range(0, firstsLines.Count - 1)];
        } else {
                name = firstsLines[Random.Range(0, firstsLines.Count - 1)] + " " + secondsLines[Random.Range(0, secondsLines.Count - 1)] + " " + thirdsLines[Random.Range(0, thirdsLines.Count - 1)];
        }


        name = System.Text.RegularExpressions.Regex.Replace(name, "[^\\w\\._ ']", "");

        return name;
    }

    public string GenerateShittyName() {
        List<string> firstNameSyllables = new List<string>();

        string name = "";

        firstNameSyllables.Add("mon");
        firstNameSyllables.Add("fay");
        firstNameSyllables.Add("shi");
        firstNameSyllables.Add("zag");
        firstNameSyllables.Add("blarg");
        firstNameSyllables.Add("rash");
        firstNameSyllables.Add("izen");

        for (int i = 0; i < firstNameSyllables.Count; i++) {
            name += firstNameSyllables[Random.Range(0, firstNameSyllables.Count)];

        }

        string firstLetter = "";
        firstLetter = name.Substring(0, 1);
        name = name.Remove(0, 1);
        firstLetter = firstLetter.ToUpper();
        name = firstLetter + name;

        return name;
    }
}
