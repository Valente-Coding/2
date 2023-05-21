using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
[Space]
    [SerializeField] private RectTransform _scorePanel;
    [SerializeField] private TMP_Text _scoreTxt;
[Space]
    [SerializeField] private RectTransform _alertPanel;
    [SerializeField] private TMP_Text _alertTxt;
[Space]
    [SerializeField] private RectTransform _introPanel;
    [SerializeField] private TMP_Text _introText1;
    [SerializeField] private TMP_Text _introText2;
    private int _introStep= 0;
    public static UIManager instance;
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    private void Start() => DisplayIntro();

    ///
    public void DisplayIntro()
    {
        _introStep = 0;
        _input.InteractCanceledEvent += StepIntro;
        _introPanel.gameObject.SetActive(true);
    }

    private void StepIntro()
    {
        _introStep ++;
        if(_introStep == 1)
        {
            _introText1.gameObject.SetActive(false);
            _introText2.gameObject.SetActive(true);

        }
        if(_introStep == 2)
        {
            _introText2.gameObject.SetActive(false);
            HideIntro();
        }
    }
    private void HideIntro()
    {
       _input.InteractCanceledEvent -= StepIntro;
       _introPanel.gameObject.SetActive(false);
       DisplayAlertMsg("Press start/enter to get to the saloon",5f);
    }

///
    public void DisplayAlertMsg(string msg, float duration)
    {
        _alertTxt.text = msg;
        StartCoroutine(COR_Alert(duration));
    }

    private IEnumerator COR_Alert(float duration)
    {
        _alertPanel.gameObject.SetActive(true);
        yield return Yielders.Get(duration);
        _alertPanel.gameObject.SetActive(false);
    }
///
    public void DisplayScore()
    {
        _input.InteractCanceledEvent += Leave;
        _scoreTxt.text = GameManager.instance.Score.TotalScore.ToString();
        _scorePanel.gameObject.SetActive(true);
    }

    private void Leave()
    {
        _input.InteractCanceledEvent -= Leave;
        GameManager.instance.SceneLoader.LoadNewScene("Menu");
    }
}
