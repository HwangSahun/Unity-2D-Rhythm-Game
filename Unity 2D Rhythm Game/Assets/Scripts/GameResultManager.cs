using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameResultManager : MonoBehaviour
{
    public Text musicTitleUI;
    public Text scoreUI;
    public Text maxComboUI;
    public Image RankUI;

    void Start()
    {
        musicTitleUI.text = PlayerInformation.musicTitle;
        scoreUI.text = "Score : " + (int) PlayerInformation.score;
        maxComboUI.text = "Max Combo : " + PlayerInformation.maxCombo;
        // 리소스에서 비트 텍스트 파일을 불러옴
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + PlayerInformation.selectedMusic);
        StringReader reader = new StringReader(textAsset.text);
        // 첫번째 줄과 두번째 줄을 무시
        reader.ReadLine();
        reader.ReadLine();
        // 세번째 줄에 적힌 비트정보 읽기
        string beatInformation = reader.ReadLine();
        int scoreS = Convert.ToInt32(beatInformation.Split(' ')[3]);
        int scoreA = Convert.ToInt32(beatInformation.Split(' ')[4]);
        int scoreB = Convert.ToInt32(beatInformation.Split(' ')[5]);
        // 성적에 맞는 이미지 불러오기
        if (PlayerInformation.score >= scoreS)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank S");
        }
        else if (PlayerInformation.score >= scoreA)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank A");
        }
        else if (PlayerInformation.score >= scoreB)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank B");
        }
        else
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank C");
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene("SongSelectScene");
    }
}
