using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class GatherCredits : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    void Start()
    {
        text.text = "";
        string curr_path = Application.dataPath;
        var creditFiles = Directory.GetFiles(curr_path, "*credits.txt");
        foreach (string credit in creditFiles)
        {
            StreamReader reader = new StreamReader(credit);
            text.text = text.text + reader.ReadToEnd() + Environment.NewLine;
            reader.Close();
        }
        if (Directory.Exists(curr_path))
        {
            var subDirectories = Directory.GetDirectories(curr_path);
            foreach (string subDirectory in subDirectories)
            {
                DirectorySearch(subDirectory);
            }
        }
    }

    void DirectorySearch(string directory)
    {
        var creditFiles = Directory.GetFiles(directory, "*credits.txt");
        foreach (string credit in creditFiles)
        {
            StreamReader reader = new StreamReader(credit);
            text.text = text.text + reader.ReadToEnd() + Environment.NewLine;
            reader.Close();
        }
        if (Directory.Exists(directory))
        {
            var subDirectories = Directory.GetDirectories(directory);
            foreach (string subDirectory in subDirectories)
            {
                DirectorySearch(subDirectory);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
