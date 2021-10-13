//몇개의 노트가 내려갈지 설정

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // 이중 리스트 사용
    // Note 1: 10개 => 리스트 1
    // Note 2: 10개 => 리스트 2
    // Note 3: 10개 => 리스트 3
    // Note 4: 10개 => 리스트 4

    public List<GameObject> Notes;
    private List<List<GameObject>> poolsOfNotes;
    public int noteCount = 10;
    private bool more = true;

    void Start()
    {
        poolsOfNotes = new List<List<GameObject> >();
        for(int i = 0; i < Notes.Count; i++) // 4번 반복
        {
            poolsOfNotes.Add(new List<GameObject>());
            for(int n = 0; n < noteCount; n++) // 10번 반복
            {
                GameObject obj = Instantiate(Notes[i]);
                obj.SetActive(false);
                poolsOfNotes[i].Add(obj);
            }
        }
    }

    public GameObject getObject(int notetype)
    {
        foreach(GameObject obj in poolsOfNotes[notetype - 1])
        {
            if(!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        if(more)
        {
            GameObject obj = Instantiate(Notes[notetype - 1]);
            poolsOfNotes[notetype - 1].Add(obj);
            return obj;
        }
        return null;
    }

    void Update()
    {
        
    }
}
