using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keymaps
{
    public class Keymap_names
    {
        private static Keymap_names instance = null;
        private string file_name;
        public bool isRTL { get; }
        public ArrayList subtitleRows;
        public string language;

        private Keymap_names()
        {
            language = LanguageHelper.Get2LetterISOCodeFromSystemLanguage();
            if (language.Equals("he") || language.Equals("ar"))
                isRTL = true;
            else isRTL = false;
            language = "he";
            file_name = "subtitles." + language.ToString();
            TextAsset textAsset = Resources.Load<TextAsset>(file_name);
            Debug.Log(textAsset.text);
            SubtitlesManager a = new SubtitlesManager();
            a = JsonUtility.FromJson<SubtitlesManager>(textAsset.text);
            Debug.Log(a.rows[0].line);
            /*subtitleRows = new ArrayList(.rows);
            Debug.Log(((SubtitleRow)subtitleRows[0]).line);*/
        }

        public static Keymap_names GetInstance()
        {
            if (instance == null)
                instance = new Keymap_names();
            return instance;
        }

        public SubtitleRow[] GetSubtitleIterator()
        {
            return (SubtitleRow[])subtitleRows.ToArray();
        }
/*
        public void keymap_names_init(string selected_language)
        {
            language = selected_language;

            if (selected_language.Equals("Hebrow"))
                loadHebrew();
            else loadEnglish();
        }

        // -- utils: load the english or hebrow files -- //
        private void loadEnglish()
        {
            lines["ramthnia_name"] = "רמתנייה";
        }

        private void loadHebrew()
        {
            lines["ramthnia_name"] = "רמתנייה";
        }

        // -- Getters & Setters -- //
        public void setLanguage(string selected_language)
        {
            language = selected_language;
        }


        public string getLanguage()
        {
            return language;
        }

        public string getValue(string key)
        {
            if (language == "Hebrow")
                return lines[key];

            return lines[key];
        }*/
    }
}