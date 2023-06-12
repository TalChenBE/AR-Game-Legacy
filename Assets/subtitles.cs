using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class Subtitles : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtitleText = default;
    public HideVideo hideVideo;

    public static Subtitles instance;
    private string file_name;
    public bool isRTL { get; set; }
    public SubtitlesManager subtitleRows;
    public string language;

    private bool oneEnter;

    private void Awake()
    {
        instance = this;
        subtitleRows = new SubtitlesManager();
        ClearSubtitles();
        language = LanguageHelper.Get2LetterISOCodeFromSystemLanguage();
        language = "he";
        if (language.Equals("he") || language.Equals("ar"))
            isRTL = true;
        else isRTL = false;
        subtitleText.isRightToLeftText = isRTL;
        file_name = "subtitles." + language.ToString();
        TextAsset textAsset = Resources.Load<TextAsset>(file_name);
        subtitleRows = JsonUtility.FromJson<SubtitlesManager>(textAsset.text);

        oneEnter = false;
    }

    private void Update()
    {
        if (hideVideo.isDestroy == false)
            return;
        else if (!oneEnter)
        {
            System.Collections.IEnumerator enumerator = (subtitleRows.rows).GetEnumerator();
            if (enumerator.MoveNext())
                SetSubtitles(enumerator);
            else ClearSubtitles();
            oneEnter = true;
        }
    }

    public void SetSubtitles(System.Collections.IEnumerator enumerator)
    {
        subtitleText.text = ((SubtitleRow)enumerator.Current).line;
        StartCoroutine(ClearAfterSeconds(((SubtitleRow)enumerator.Current).durationSec, enumerator));
    }

    public void SetIsRightToLeft(bool isRightToLeft)
    {
        subtitleText.isRightToLeftText = isRightToLeft;
    }
    
    public void ClearSubtitles()
    {
        subtitleText.text = "";
    }

    private IEnumerator ClearAfterSeconds(float delay, System.Collections.IEnumerator enumerator)
    {
        yield return new WaitForSeconds(delay);
        ClearSubtitles();
        if(enumerator.MoveNext())
            SetSubtitles(enumerator);
    }
}
