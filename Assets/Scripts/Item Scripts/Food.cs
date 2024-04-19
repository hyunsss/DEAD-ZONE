using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Food : Item, IUseable, IDurable
{
    [SerializeField] private int durability;
    [SerializeField] private int maxDurability;

    public int Durability { get => durability; set => durability = value; }
    public int MaxDurability => maxDurability;

    [SerializeField] private float delay;
    public float Delay { get => delay;}

    public float energy;
    public float moisture;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use(float value) {
        PlayerManager.status.Hungry += energy * value;
        PlayerManager.status.Moisture += moisture * value;

        Durability -= (int)(Durability * value);
        energy -= energy * value;
        moisture -= moisture * value;
    }

    public void ShowSetUsageValuePanel() {
        //UIManager에서 띄우도록 내구도 설정하는 패널 팝업을 띄우도록 하고 
        //현재 아이템을 파라미터로 넘겨 수치를 설정 할 수 있게함.
        //현재 아이템의 내구도만큼을 최대 밸류로 설정해주고 
        //슬라이더를 움직인 만큼의 내구도를 사용할 수 있도록 설정
        //최대내구도 대비 사용자가 움직인 만큼의 내구도 비율만큼 에너지와 수분을 적용시키도록 하며
        //적용 된 후 수치는 차감해야 함.
        //차감 되는 수치 : energy, moisture, durability 
        //DurabilityItem class에서 내구도가 0이 될경우 자동으로 아이템을 삭제하도록 함. 

        //내구도 설정 패널 팝업에서는 confirm, cancel 버튼으로 확인 및 취소를 할 수 있고,
        //확인을 누르면 IUseable의 Use() 메서드를 호출할 수 있게함. 
        //취소하면 팝업 Despawn

        UIManager.Instance.current_usageValueSetting = LeanPool.Spawn(UIManager.Instance.usageValueSetting_prefab, UIManager.Instance.PopUpTransform);
        UIManager.Instance.current_usageValueSetting.currentItem = this;
        UIManager.Instance.current_usageValueSetting.Init();
    }
}
