using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RootingBox : MonoBehaviour, IInteractable
{
    public string box_Name;

    public List<Item> keyMatchItemList = new List<Item>();
    UIElementClickHandler[] currentBoxItems;
    private int itemCount;
    public int minitemCount, maxitemCount;
    public ItemCellPanel currentItemCellPanel;
    public GameObject cellPanelPrefab;
    public ItemKey allowItemKey;

    bool isSearch;

    private Coroutine SearchCoroutine;

    public InteractType Type { get => InteractType.RootBox; }

    private void Awake()
    {
        itemCount = Random.Range(minitemCount, maxitemCount);
        currentItemCellPanel = LeanPool.Spawn(cellPanelPrefab).GetComponent<ItemCellPanel>();
        currentItemCellPanel.boxType = BoxType.RootBox;
    }

    private void Start()
    {
        keyMatchItemList = ItemManager.Instance.SearchListbyKey(allowItemKey);
        if (currentItemCellPanel.grid.isInit == false)
        {
            currentItemCellPanel.grid.GenerateGrid();
        }
        GetBoxItems();
    }

    private void OnEnable() {
        RectTransform rect = currentItemCellPanel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.position = new Vector2(25, -25f);
    }

    private void OnDisable() {
        RectTransform rect = currentItemCellPanel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.position = Vector2.zero;
    }

    public void StopMyCoroutine()
    {
        if (SearchCoroutine != null)
        {
            StopCoroutine(SearchCoroutine);
        }
    }

    public void GetBoxItems()
    {
        for (int i = 0; i < itemCount; i++)
        {
            int index = Random.Range(0, keyMatchItemList.Count);
            BoxInItem(keyMatchItemList[index], out bool success);
            if (success == true)
            {
                continue;
            }
            else
            {
                break;
            }
        }
    }

    IEnumerator SearchItem()
    {

        int i = 0;
        while (currentBoxItems.Length > i)
        {
            if (currentBoxItems[i].myItem.isSearchable == false)
            {
                float delay = Random.Range(2f, 3.3f);
                LoadingUI loadingUI = LeanPool.Spawn(UIManager.Instance.loadingUI_prefab, currentBoxItems[i].transform, false).GetComponent<LoadingUI>();
                loadingUI.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;


                yield return new WaitForSeconds(delay);
                currentBoxItems[i].myItem.isSearchable = true;
                Image item_image = currentBoxItems[i].GetComponent<Image>();
                item_image.enabled = true;
                item_image.raycastTarget = true;

                if (currentBoxItems[i].myItem.item_Type != ItemType.None)
                {
                    currentBoxItems[i].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                }


                loadingUI.Destroy();
                UIManager.Instance.loadingUI_list.Remove(loadingUI);
                i++;
            }
            else
            {
                i++;
                continue;
            }
        }

        yield return null;
    }

    public void BoxInItem(Item item, out bool success)
    {
        GameObject item_obj = LeanPool.Spawn(item, Vector3.zero, Quaternion.identity, ItemManager.Instance.itemParent).gameObject;

        if (item_obj.TryGetComponent(out Bag bag))
        {
            ItemCellPanel bag_Inven = bag.currentBagInventory.GetComponent<ItemCellPanel>();
            if (bag_Inven.grid == null)
            {
                bag_Inven.grid = new GridXY(bag_Inven.width, bag_Inven.height, bag_Inven.transform);
            }
            if (bag_Inven.grid.isInit == false)
            {
                bag_Inven.grid.GenerateGrid();
            }
        }
        else if (item_obj.TryGetComponent(out Armor armor))
        {
            ItemCellPanel[] armor_Invens = armor.currentRigInventory.GetComponentsInChildren<ItemCellPanel>();

            foreach (ItemCellPanel armor_Inven in armor_Invens)
            {
                if (armor_Inven.grid == null)
                {
                    armor_Inven.grid = new GridXY(armor_Inven.width, armor_Inven.height, armor_Inven.transform);
                }
                if (armor_Inven.grid.isInit == false)
                {
                    armor_Inven.grid.GenerateGrid();
                }
            }

        }
        else if (item_obj.TryGetComponent(out Ammo ammo))
        {
            int itemcount = Random.Range(8, 35);
            ammo.Count = itemcount;
        }
        else if (item_obj.TryGetComponent(out Money money)) {
            int itemcount = Random.Range(500, 9999);
            money.Count = itemcount;
        }

        ItemManager.Instance.MoveToInventoryFindCell(currentItemCellPanel.grid, item_obj.GetComponent<Item>(), out bool Finish);

        if (Finish == false)
        {
            success = false;
            LeanPool.Despawn(item_obj);
        }
        else
        {
            success = true;
            item_obj.GetComponent<Item>().isSearchable = false;
        }
    }

    /*
        TodoList :: 
        1. 아이템이 만약 isSearchable == false 라면 인벤토리에 들어갔을 경우 Icon Image를 비활성화 해주어 해당 아이템이 어떤 아이템인지 보이지 않게 함.
        2. 드랍 불가능, 드래그 불가능 
        3. isSearchable == true일 경우는 원래 상태대로 진행
        4. 찾기 버튼을 누를 경우 해당 CellPanel에 있는 아이템들의 리스트를 받고 순차적으로 아이템 찾기를 진행하며 일정 시간이 지날경우 아이템의 이미지, 타겟레이캐스트를 활성해 해준다. 
    */

    public void Interact()
    {

        currentItemCellPanel.transform.SetParent(UIManager.Instance.rooting_transform, false);
        currentBoxItems = currentItemCellPanel.GetComponentsInChildren<UIElementClickHandler>();

        UIManager.Instance.rooting_transform.gameObject.SetActive(true);

        UIManager.Instance.currentRootBox = this;
        UIManager.Instance.ShowPlayerInventory();
        SetSearchableItem();

        UIManager.Instance.searchButton.onClick.RemoveAllListeners();
        UIManager.Instance.searchButton.onClick.AddListener(() => SearchCoroutine = StartCoroutine(SearchItem()));
    }

    void SetSearchableItem()
    {
        foreach (var item_icon in currentBoxItems)
        {
            if (item_icon.myItem.isSearchable == false)
            {
                Image item_image = item_icon.GetComponent<Image>();
                item_image.raycastTarget = false;
                item_image.enabled = false;

                if (item_icon.myItem.item_Type == ItemType.StackableItem || item_icon.myItem.item_Type == ItemType.DurableItem)
                {
                    item_icon.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                }
            }
        }
    }
}
