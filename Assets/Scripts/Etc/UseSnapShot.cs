using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UseSanpShot : MonoBehaviour
{
    // 받은 스냅샷 카메라 스크립트
    private SnapshotCamera snapshotCamera;
    // 찍을 오브젝트를 배열로 입력 받음
    public GameObject[] gameObjectToSnapshot;

    // 파일 주소 설정
    readonly string Path = Application.dataPath + "/Textures";
    // 파일 이름
    string Name;

    void Start()
    {
        // 카메라 설정
        snapshotCamera = SnapshotCamera.MakeSnapshotCamera(0);
        for (int i = 0; i < gameObjectToSnapshot.Length; i++)
        {
            // 파일 이름 설정
            Name = gameObjectToSnapshot[i].gameObject.name + "_Icon";
            // 사진 찍기
            Texture2D snapshot = snapshotCamera.TakePrefabSnapshot(gameObjectToSnapshot[i]);
            // 사진 저장
            SnapshotCamera.SavePNG(snapshot, Name, Path);
        }
    }

    public void GetPictureFunc(GameObject[] gameObjects, int width, int height) {
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