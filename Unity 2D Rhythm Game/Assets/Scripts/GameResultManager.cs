using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GameResultManager : MonoBehaviour
{
    public Text musicTitleUI;
    public Text scoreUI;
    public Text maxComboUI;
    public Image RankUI;

    public Text rank1UI;
    public Text rank2UI;
    public Text rank3UI;

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
        rank1UI.text = "Loading Data";
        rank2UI.text = "Loading Data";
        rank3UI.text = "Loading Data";
        DatabaseReference reference = PlayerInformation.GetDatabaseReference().Child("ranks")
            .Child(PlayerInformation.selectedMusic);
        // 데이터 셋의 모든 데이터를 JSON 형태로 불러옴
        reference.OrderByChild("score").GetValueAsync().ContinueWith(task =>
        {
            // 성공적으로 데이터를 가져온 경우
            if (task.IsCompleted)
            {
                List<string> rankList = new List<string>();
                List<string> emailList = new List<string>();
                DataSnapshot snapshot = task.Result;
                // JSON 데이터의 각 원소에 접근
                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary rank = (IDictionary)data.Value;
                    emailList.Add(rank["email"].ToString());
                    rankList.Add(rank["score"].ToString());
                }
                // 정렬 이후 순서를 뒤집어 내림차순 정렬
                emailList.Reverse();
                rankList.Reverse();
                // 최대 상위 3명의 순위를 차례대로 화면에 출력
                rank1UI.text = "No User Play";
                rank2UI.text = "No User Play";
                rank3UI.text = "No User Play";
                List<Text> textList = new List<Text>();
                textList.Add(rank1UI);
                textList.Add(rank2UI);
                textList.Add(rank3UI);
                int count = 1;
                for(int i = 0; i < rankList.Count && i < 3; i++)
                {
                    textList[i].text = count + "rank: " + emailList[i] + " (" + rankList[i] + " score)";
                    count = count + 1;
                }
            }
        });
    }

    public void Replay()
    {
        SceneManager.LoadScene("SongSelectScene");
    }
}
