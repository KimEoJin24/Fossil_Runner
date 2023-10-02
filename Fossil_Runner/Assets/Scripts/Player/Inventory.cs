using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class ItemSlot
{
    public ItemData item;  //아이템 데이더가 있는 곳
    public int quantity;  // ?
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;   // 아이템 정보 넣을려는 배열 인듯
    public ItemSlot[] slots;   // 슬롯들 모아놓을려는 배열 인듯


    public GameObject inventoryWindow;  //인벤토리창
    public Transform dropPosition;  //떨어트리는 위치


    [Header("Selected Item")]
    private ItemSlot selectedItem;  // 선택한아이템 담는 변수?
    private int selectedItemIndex;  // 선택한 슬롯 인덱스
    public TextMeshProUGUI selectedItemName; 
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatNames;
    public TextMeshProUGUI selectedItemStatValues;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerConditions condition;

    [Header("Events")]
    public UnityEvent onOpenInventory;  //열때 발생하는 이벤트
    public UnityEvent onCloseInventory; // 닫을 때 발생하는 이벤트

    public static Inventory instance;  //싱글톤?
    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerConditions>();
    }
    private void Start()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)  //각각의 인벤토리에 슬롯 번호와 초기화를 시켜줌
        {
            slots[i] = new ItemSlot(); 
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        ClearSeletecItemWindow();
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext) //인벤토리를 켜면?
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }


    public void Toggle() // 켯다 껏다하는 메서드
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory?.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory?.Invoke();
            controller.ToggleCursor(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy; //부모오브젝트의 경우를 대비하여 오브젝트의 상태를 확인할 경우
    }

    public void AddItem(ItemData item)
    {
        if (item.canStack) //아이템을 쌓을 수 있다면
        {
            ItemSlot slotToStackTo = GetItemStack(item); //아이템 쌓기
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++; //아이템 쌓기
                UpdateUI(); //업로드하기
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();  //빈슬롯을 찾는 메서드

        if (emptySlot != null) //슬롯이 있다면 -> 이건 스택이 불가한경우
        {
            emptySlot.item = item; //아이템 추가
            emptySlot.quantity = 1; // 1
            UpdateUI();//업로드
            return;
        }

        ThrowItem(item); //둘다아니면 못먹어요
    }

    void ThrowItem(ItemData item)  //아이템 버리는 메서드
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) //아이템이 있으면 설정
                uiSlots[i].Set(slots[i]);
            else
                uiSlots[i].Clear();  //아이템이 없으면 클리어
        }
    }

    ItemSlot GetItemStack(ItemData item)  //스택쌓기
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount) //아이템의 종류거 같으면서 최대치를 넘지 않는다면
                return slots[i];
        }

        return null;
    }

    ItemSlot GetEmptySlot() //빈슬룻 찾아서 넣기
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }

    public void SelectItem(int index)  //
    {
        if (slots[index].item == null)
            return;

        selectedItem = slots[index]; //슬롯을 찾음
        selectedItemIndex = index;  // 슬롯번호

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++) //길면뛰는 메서드인듯
        {
            selectedItemStatNames.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValues.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    private void ClearSeletecItemWindow()  //선택한아이템 지우기
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;

        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumables[i].value); break;
                    case ConsumableType.Thirsty:
                        condition.Drink(selectedItem.item.consumables[i].value); break;

                }
            }

            RemoveSelectedItem();
        }

        void OnEquipButton()
        {

        }

        void UnEquip(int index)
        {

        }

        void OnUnEquipButton()
        {

        }

        void OnDropButton()
        {
            ThrowItem(selectedItem.item);
            RemoveSelectedItem();
        }

        void RemoveSelectedItem() //아이템 제거하기
        {
            selectedItem.quantity--;

            if (selectedItem.quantity <= 0)
            {
                if (uiSlots[selectedItemIndex].equipped)
                {
                    UnEquip(selectedItemIndex);
                }

                selectedItem.item = null;
                ClearSeletecItemWindow();
            }

            UpdateUI();
        }

        void RemoveItem(ItemData item)
        {

        }

        bool HasItems(ItemData item, int quantity)
        {
            return false;
        }
    }
}
