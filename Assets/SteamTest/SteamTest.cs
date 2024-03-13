using System.Collections;
using System.Collections.Generic;
using SteamImage = Steamworks.Data.Image;
using UnityImage = UnityEngine.UI.Image;
using UnityEngine;
using Steamworks;
using Unity.VisualScripting;

public class SteamTest : MonoBehaviour
{
    public UnityImage avartarImage1;

    public Transform friendtrans;

    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    async void Start()
    {
        SteamClient.Init(480);

        print(SteamClient.Name);

        SteamImage? avartarNullableImage = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);

        if(avartarNullableImage.HasValue)
        {
            avartarImage1.sprite = SteamImageToSprite(avartarNullableImage);
        }
        else
        {
            avartarImage1.sprite = null;
        }

        foreach (var friend in SteamFriends.GetFriends())
        {
            SteamImage? friendImage = await SteamFriends.GetLargeAvatarAsync(friend.Id);

            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(friendtrans);
            UnityImage image = gameObject.AddComponent<UnityImage>();
            gameObject.name = friend.Name;
            image.sprite = SteamImageToSprite(friendImage);
        }

    }

    Sprite SteamImageToSprite(SteamImage? avartarNullableImage)
    {
        SteamImage avartarImage;

        avartarImage = avartarNullableImage.Value;
        Texture2D avartarTexture = new Texture2D((int)avartarImage.Width, (int)avartarImage.Height, TextureFormat.ARGB32, false);
        avartarTexture.filterMode = FilterMode.Trilinear;

        for (int x = 0; x < avartarImage.Width; x++)
        {
            for (int y = 0; y < avartarImage.Height; y++)
            {
                var colorPixel = avartarImage.GetPixel(x, y);
                avartarTexture.SetPixel(x, (int)avartarImage.Height - y, new Color(colorPixel.r / 255f, colorPixel.g / 255f, colorPixel.b / 255f , colorPixel.a / 255f));
            }
        }

        avartarTexture.Apply();

        Sprite avartarsprite = Sprite.Create(avartarTexture, new Rect(0, 0, avartarTexture.width, avartarTexture.height), new Vector2(0.5f, 0.5f));

        return avartarsprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
