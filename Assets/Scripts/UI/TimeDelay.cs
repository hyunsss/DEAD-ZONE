using System;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeDelay : MonoBehaviour
{
    bool isInit;
    public float delay;
    private float remainTime;

    public Action<float> UseMethod;
    public float action_value;

    public TextMeshProUGUI timeText;
    public Image fillImage;

    private float startTime;

    // Start is called before the first frame update
    public void Init(float delay, float action_value, Action<float> useMethod)
    {
        this.delay = delay;
        this.action_value = action_value;
        UseMethod += useMethod;
        remainTime = delay;

        isInit = true;

        startTime = Time.time;
        PlayerManager.move.WalkSpeed -= 1f;
        transform.SetAsFirstSibling();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit == true)
        {
            if (remainTime > 0)
            {
                

                remainTime = delay - (Time.time - startTime);
                timeText.text = Math.Round(remainTime, 1).ToString();

                float fill = 1 - (remainTime / delay);
                fillImage.fillAmount = fill;
            }
            else
            {
                remainTime = 0;
                UseMethod?.Invoke(action_value);
                Reset();
                LeanPool.Despawn(this);
            }
        }
    }

    private void Reset() {
        PlayerManager.move.WalkSpeed += 1f;
        isInit = false;
        UseMethod = null;
        delay = 0;
        fillImage.fillAmount = 0;
        timeText.text = "";
    }
}
