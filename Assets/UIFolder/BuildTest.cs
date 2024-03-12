using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildTest : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
#if DEVELOPMENT_BUILD
    text.text = "development build";
#elif TEST
    text.text = "test build";
#endif

#if TEST && UNITY_EDITOR
    text.text = "build";
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
