using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _scorePanel;
    [SerializeField] private TMP_Text _scoreTxt;
    [SerializeField] private RectTransform _alertPanel;
    [SerializeField] private TMP_Text _alertTxt;
    public static UIManager instance;
    public static UIManager Input;

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

    public void DisplayAlertMsg(string msg, float duration)
    {
        _alertTxt.text = msg;
        StartCoroutine(COR_Alert(duration));
    }

    private IEnumerator COR_Alert(float duration)
    {
        _alertPanel.gameObject.SetActive(true);
        yield return Yielders.Get(duration);
        _alertTxt.text = "!!";
        yield return Yielders.Get(1.5f);
        _alertPanel.gameObject.SetActive(false);
    }

    public void DisplayScore()
    {
        _scoreTxt.text = GameManager.instance.Score.TotalScore.ToString();
        _scorePanel.gameObject.SetActive(true);
    }
}
