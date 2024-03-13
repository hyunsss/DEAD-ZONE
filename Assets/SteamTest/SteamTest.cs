using System.Collections;
using System.Collections.Generic;
using SteamImage = Steamworks.Data.Image;
using UnityImage = UnityEngine.UI.Image;
using UnityEngine;
using Steamworks;

public class SteamTest : MonoBehaviour
{
    public UnityImage avartarImage;
    // Start is called before the first frame update
    void Start()
    {
        SteamClient.Init(480);

        print(SteamClient.Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
