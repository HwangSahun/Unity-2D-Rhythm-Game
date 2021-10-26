//게임 매니저를 싱글 톤 처리
//여러 c#에 관해 관리

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 게임 매니저를 싱글 톤 처리_원활한 게임 관리 떄문
    public static GameManager instance { get; set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public float noteSpeed;

    public GameObject scoreUI;
    private float score;
    private Text scoreText;

    public GameObject comboUI;
    private int combo;
    private Text comboText;
    private Animator comboAnimator;

    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };
    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;

    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRendereres;

    void Start()
    {
        judgementSpriteRenderer = judgeUI.GetComponent<Image>();
        judgementSpriteAnimator = judgeUI.GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<Text>();
        comboText = comboUI.GetComponent<Text>();
        comboAnimator = comboUI.GetComponent<Animator>();

        // 판정 결과를 보여주는 스프라이트 이미지 초기화
        judgeSprites = new Sprite[4];
        judgeSprites[0] = Resources.Load<Sprite>("Sprites/Bad");
        judgeSprites[1] = Resources.Load<Sprite>("Sprites/Good");
        judgeSprites[2] = Resources.Load<Sprite>("Sprites/Miss");
        judgeSprites[3] = Resources.Load<Sprite>("Sprites/Perfect");

        trailSpriteRendereres = new SpriteRenderer[trails.Length];
        for(int i = 0; i < trails.Length; i++)
        {
            trailSpriteRendereres[i] = trails[i].GetComponent<SpriteRenderer>();
        }
    }
    
    void Update()
    {
        // 사용자가 입력한 키에 해당하는 라인을 빛나게 처리
        if (Input.GetKey(KeyCode.D)) ShineTrail(0);
        if (Input.GetKey(KeyCode.F)) ShineTrail(1);
        if (Input.GetKey(KeyCode.J)) ShineTrail(2);
        if (Input.GetKey(KeyCode.K)) ShineTrail(3);

        for(int i = 0; i < trailSpriteRendereres.Length; i++)
        {
            Color color = trailSpriteRendereres[i].color;
            color.a -= 0.01f;
            trailSpriteRendereres[i].color = color;
        }
    }

    public void ShineTrail(int index)
    {
        Color color = trailSpriteRendereres[index].color;
        color.a = 0.32f;
        trailSpriteRendereres[index].color = color;
    }

    // 노트 판정 이후에 판정 결과를 화면에 보여줌
    void showJudgement()
    {
        // 점수 이미지를 보여줌
        string scoreFormat = "000000";
        scoreText.text = score.ToString(scoreFormat);

        // 판정 이미지를 보여줌
        judgementSpriteAnimator.SetTrigger("Show");
        // 콤보가 2 이상일 때만 콤보 이미지를 보여줌
        if(combo >= 2)
        {
            comboText.text = "COMBO" + combo.ToString();
            comboAnimator.SetTrigger("Show");
        }
    }

    // 노트 판정을 진행
    public void processJudge(judges judge, int noteType)
    {
        if (judge == judges.NONE) return;
        // MISS 판정을 받은 경우 콤보 종료, 점수를 많이 깎음
        if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            if (score >= 15) score -= 15;
        }
        // BAD 판정을 받은 경우 콤보를 종료, 점수를 조금 깎음
        else if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            if (score >= 5) score -= 5;
        }
        // PERFECT 혹은 GOOD 판정을 받은 경우 콤보 및 점수를 올림
        else
        {
            if (judge == judges.PERFECT)
            {
                judgementSpriteRenderer.sprite = judgeSprites[3];
                score += 20;
            }
            else if (judge == judges.GOOD)
            {
                judgementSpriteRenderer.sprite = judgeSprites[1];
                score += 15;
            }
            combo += 1;
            score += (float)combo * 0.1f;
        }
        showJudgement();
    }
}
