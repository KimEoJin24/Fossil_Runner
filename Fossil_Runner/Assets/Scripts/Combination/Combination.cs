using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Combination : MonoBehaviour
{
    public CombinationSlotUI[] uiSlots;   // 아이템 정보 넣을려는 배열 인듯
    public ItemSlot[] slots;   // 슬롯들 모아놓을려는 배열 인듯
    public ItemData[] datas;

    public static Combination instance;  //싱글톤?
    // Start is called before the first frame update
   // private ItemSlot selectedItem;  // 선택한아이템 담는 변수?
   // private int selectedItemIndex;  // 선택한 슬롯 인덱스
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatNames;
    public TextMeshProUGUI selectedItemStatValues;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        //슬롯창의 정보를 담기로 하기
        //선택한 슬룻의 설명창이 보이도록 구현하기 
        slots = new ItemSlot[uiSlots.Length];
        //icon.sprite = slot.item.icon;
        for (int i = 0; i < slots.Length; i++)  //각각의 인벤토리에 슬롯 번호와 초기화를 시켜줌
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].icon.sprite = datas[i].icon; 
           // uiSlots[i].Clear();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectItem(int index)  // 누른 것에 대한 인덱스 번호만 찾아줘용
    {


        //if (slots[index].item == null)
        //    return;

        //selectedItem = slots[index]; //슬롯을 찾음
        //selectedItemIndex = index;  // 슬롯번호

        selectedItemName.text = datas[index].name;
        selectedItemDescription.text = datas[index].description;

        // 몇개의 재료가 있는지 확인하기
        // 데이터 확인하기 설명란에? 추가하기 가능하다면 창 활성화 띄우고 기기
        //

        //selectedItemStatNames.text = string.Empty;
        //selectedItemStatValues.text = string.Empty;

        //for (int i = 0; i < selectedItem.item.consumables.Length; i++) //길면뛰는 메서드인듯
        //{
        //    selectedItemStatNames.text += selectedItem.item.consumables[i].type.ToString() + "\n";
        //    selectedItemStatValues.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        //}

        //useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        //equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        //unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        //dropButton.SetActive(true);
    }
}
