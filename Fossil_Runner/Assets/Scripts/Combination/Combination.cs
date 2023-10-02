using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Combination : MonoBehaviour
{
    public CombinationSlotUI[] uiSlots;   // ������ ���� �������� �迭 �ε�
    public ItemSlot[] slots;   // ���Ե� ��Ƴ������� �迭 �ε�
    public ItemData[] datas;

    public static Combination instance;  //�̱���?
    // Start is called before the first frame update
   // private ItemSlot selectedItem;  // �����Ѿ����� ��� ����?
   // private int selectedItemIndex;  // ������ ���� �ε���
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
        //����â�� ������ ���� �ϱ�
        //������ ������ ����â�� ���̵��� �����ϱ� 
        slots = new ItemSlot[uiSlots.Length];
        //icon.sprite = slot.item.icon;
        for (int i = 0; i < slots.Length; i++)  //������ �κ��丮�� ���� ��ȣ�� �ʱ�ȭ�� ������
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
    public void SelectItem(int index)  // ���� �Ϳ� ���� �ε��� ��ȣ�� ã�����
    {


        //if (slots[index].item == null)
        //    return;

        //selectedItem = slots[index]; //������ ã��
        //selectedItemIndex = index;  // ���Թ�ȣ

        selectedItemName.text = datas[index].name;
        selectedItemDescription.text = datas[index].description;

        // ��� ��ᰡ �ִ��� Ȯ���ϱ�
        // ������ Ȯ���ϱ� �������? �߰��ϱ� �����ϴٸ� â Ȱ��ȭ ���� ���
        //

        //selectedItemStatNames.text = string.Empty;
        //selectedItemStatValues.text = string.Empty;

        //for (int i = 0; i < selectedItem.item.consumables.Length; i++) //���ٴ� �޼����ε�
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
