using System;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class GatherCredits : IPreprocessBuildWithReport
    {
        // Start is called before the first frame update
        public Text text;
        private string _textAccumulator;

        public int callbackOrder { get; }
        public void OnPreprocessBuild(BuildReport report)
        {
            _textAccumulator = "";
            string currPath = Application.dataPath;
            DirectorySearch(currPath);

            var file = new StreamWriter(Application.dataPath + "/Resources/Credits.txt");
            file.Write(_textAccumulator.ToCharArray());
            file.Close();
        }
        
        void DirectorySearch(string directory)
        {
            var creditFiles = Directory.GetFiles(directory, "*credits.txt");
            foreach (string credit in creditFiles)
            {
                StreamReader reader = new StreamReader(credit);
                _textAccumulator += reader.ReadToEnd() + Environment.NewLine + Environment.NewLine;
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
    }
}
