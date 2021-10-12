//게임 매니저를 싱글 톤 처리
//여러 c#에 관해 관리

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public float noteSpeed;

    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
