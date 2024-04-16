using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
#region 1by1 snapshot setting

//default rotation  public Vector3 defaultRotation = new Vector3(360.8529f, 290.8297f, 40.28433f);
/*
    /// </summary>
    public Vector3 defaultPositionOffset = new Vector3(0, -0.2f, 1);
    /// <summary>
    /// The default rotation applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultRotation = new Vector3(315.8529f, 300.8297f, 37.28433f);
    /// <summary>
    /// The default scale applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultScale = new Vector3(5.2f, 5.2f, 5.2f);
*/
#endregion
#region 1by2 snapshot setting
/*
    public Vector3 defaultPositionOffset = new Vector3(0, -0.9f, 1);
    /// <summary>
    /// The default rotation applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultRotation = new Vector3(0, 0, 0);
    /// <summary>
    /// The default scale applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultScale = new Vector3(3.2f, 3.2f, 3.2f);
*/
#endregion
#region 2by1 Gun snapshot setting
/*
    public Vector3 defaultPositionOffset = new Vector3(-1f, 0, 1);
    /// <summary>
    /// The default rotation applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultRotation = new Vector3(0, 90, 0);
    /// <summary>
    /// The default scale applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultScale = new Vector3(7f, 7f, 7f);
*/
#endregion
#region 1by3 snapshot setting

#endregion
#region 2by5 Gun snapshot setting

#endregion

#region 4by2 Gun snapshot setting
/*
 /// <summary>
    /// The default position offset applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultPositionOffset = new Vector3(-0.6f, -0.2f, 1);
    /// <summary>
    /// The default rotation applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultRotation = new Vector3(0, 90, 0);
    /// <summary>
    /// The default scale applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultScale = new Vector3(4.6f, 4.6f, 4.6f);
*/
#endregion


#region 3by2 bag snapshot setting

#endregion

#region armor snapshot setting
/*
/// </summary>
    public Vector3 defaultPositionOffset = new Vector3(0f, -3.5f, 1);
    /// <summary>
    /// The default rotation applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultRotation = new Vector3(0, 200, 0);
    /// <summary>
    /// The default scale applied to objects when none is specified.
    /// </summary>
    public Vector3 defaultScale = new Vector3(2.8f, 2.8f, 2.8f);
*/
#endregion
public class UseSanpShot : MonoBehaviour
{
    public Button item1by2button;
    // 받은 스냅샷 카메라 스크립트
    private SnapshotCamera snapshotCamera;
    // 찍을 오브젝트를 배열로 입력 받음
    public GameObject[] item1by2Snapshot;

    // 파일 주소 설정
    readonly string Path = Application.dataPath + "/Textures/Magazine";
    // 파일 이름
    string Name;

    void Start()
    {
        // 카메라 설정 128 256 384 512 640
        GetPictureFunc(item1by2Snapshot, 128, 256);

    }
    //기본 128 x 128
    public void GetPictureFunc(GameObject[] gameObjects, int width = 128, int height = 128)
    {
        snapshotCamera = SnapshotCamera.MakeSnapshotCamera(0);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            // 파일 이름 설정
            Name = gameObjects[i].gameObject.name + "_Icon";
            // 사진 찍기
            Texture2D snapshot = snapshotCamera.TakePrefabSnapshot(gameObjects[i], width, height);
            // 사진 저장
            SnapshotCamera.SavePNG(snapshot, Name, Path);
        }
    }
}