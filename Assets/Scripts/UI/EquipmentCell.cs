using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentCell : Cell
{
    public enum EquipmentType { Helmat, Armor, Accesary, Weapon }
    public EquipmentType equiptype;
    //public Item slotcurrentItem;

    //장착 아이템들은 모두 장착될 트랜스폼을 가지고 있어야함 
    //장착 아이템 타입에 따라 타입이 맞지 않으면 다시 리턴시키기
    //장착 아이템에 따른 스텟 설정은 타입에 맞게 함.

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if(slotcurrentItem == null && dropped.TryGetComponent(out UIElementClickHandler component)) {
            if(IsItemAllowed(this.equiptype, component.myItem.type) == true) {
                component.dropCell = this;
            }
        }
    }

    public bool IsItemAllowed(EquipmentType cellType, ItemKey itemKey)
    {
        switch (cellType)
        {
            case EquipmentType.Helmat:
                // Helmet
                return (itemKey & (ItemKey.Helmat)) != ItemKey.Not;
            case EquipmentType.Armor:
                // Armor
                return (itemKey & ItemKey.Armor) != ItemKey.Not;
            case EquipmentType.Accesary:
                // Accessory
                return (itemKey & (ItemKey.Accesary)) != ItemKey.Not;
            case EquipmentType.Weapon:
                // Weapon
                return (itemKey & (ItemKey.Weapon)) != ItemKey.Not;
            default:
                return false;
        }
    }

    public void EquipItem(Item item)
    {
        if(IsItemAllowed(equiptype, item.type) == true) {
            //장착 로직
            /*
            1. 아이템마다 포지션을 정해서 장착해줄지는 모르겠으나 weapon의 경우 손에 장착되는 포지션이 있을테니 포지션을 설정해주는 로직을 작성해야한다. 
            2. 장착했을 때 아이템이 소환되도록하고 이것을 복제하지 않게 하기위해 장착 해제 로직에서는 해당 오브젝트를 삭제하도록 한다. 
            3. --- 아니면 플레이어 프렙스로 저장하는 방법도 괜찮을 듯.
            4. 장착하는 아이템의 경우 rigidbody같은 물리적 상호작용 컴포넌트를 비활성화 한다.
            */
        }
    }

    public void UnEquipItem()
    {
        //장착 해제 로직 
        /*
                  
        */
    }

    // Update is called once per frame
    void Update()
    {

    }
}
