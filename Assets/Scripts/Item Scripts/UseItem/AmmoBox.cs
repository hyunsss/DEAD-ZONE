using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Item, IUseable
{
    public void Use()
    {
        //총알을 지급하는 함수

        //1. UI매니저에서 플레이어의 인벤토리가 남아있는지 여부를 판단한다
        //2. 남은 인벤토리가 없을 경우 오류메세지를 출력하고 리턴한다.
        //3. 자리가 있다면 아이템매니저를 통해 지급해야하는 아이템이거나 사용해야하는 아이템일 경우 플레이어에게 할당을 해준뒤 본 아이템을 삭제시킨다.
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
