using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using static UnityEditor.Progress;

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
    public TextMeshProUGUI RequiredMaterialsName;
    public TextMeshProUGUI RequiredMaterialsNum;
    public TextMeshProUGUI MyMaterialsNum;
    public GameObject CombinationButton;
    

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
        RequiredMaterialsName.text = "";
        RequiredMaterialsNum.text = "";
        MyMaterialsNum.text = "";


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

        for (int i = 0; i < datas[index].needMatrials.Length; i++) //���ٴ� �޼����ε�
        {
            RequiredMaterialsName.text += datas[index].needMatrials[i].ToString() + "\n";
            RequiredMaterialsNum.text += datas[index].matrialsNum[i].ToString() + "\n";
           // MyMaterialsNum.text += Inventory.instance.GetItemStackNum("��������").ToString() + "\n";

            // MyMaterialsNum.text += Inventory.instance.GetItemStackNum(datas[index].needMatrials[i]).ToString() + "\n";
        }

        //useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        //equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        //unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        //dropButton.SetActive(true);
    }

    public void StackNum()
    {
        //Inventory.instance.GetItemStackNum()
    }
}
