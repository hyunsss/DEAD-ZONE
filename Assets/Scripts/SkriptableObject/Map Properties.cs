using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Properties", menuName = "Map Properties", order = 0)]
public class MapProperties : ScriptableObject {
    
    [Header("맵 오브젝트")]
    public GameObject map_Object;

    [Header("게임 내 시간")]
    public float play_Time;

    [Header("탈출구 지점 리스트")]
    public List<Transform> escape_Points;

    [Header("스폰 지점 리스트")]
    public List<Transform> spawn_Points;

}
