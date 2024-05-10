using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public float endPlayTime;
    public float playTime;
    private float startTime;

    private int min;
    private int sec;

    public int Min { get => min; }
    public int Sec { get => sec; }

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        playTime = Time.time - startTime;

        sec = (int)(60 - endPlayTime % 60) - (int)playTime % 60;
        min = (int)(endPlayTime / 60) - (int)playTime / 60 - 1;

    }

    public bool TimeOver()
    {
        return endPlayTime < playTime ? true : false;
    }
}
