using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartEncounter : MonoBehaviour
{
    public Button submitButton;
    void Start()
    {
        Button btn = submitButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        var server = GameObject.Find("Input").GetComponent<UserInput>().GetServer();
        var playerId = GameObject.Find("Input").GetComponent<UserInput>().GetPlayerId();
        PlayerPrefs.SetString("playerId", playerId);
        PlayerPrefs.SetString("server", server);
        PlayerPrefs.Save();
        SceneManager.LoadScene("BattleField");
    }
}
