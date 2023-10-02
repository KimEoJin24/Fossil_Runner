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
    public ItemData item;  //������ ���̴��� �ִ� ��
    public int quantity;  // ?
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;   // ������ ���� �������� �迭 �ε�
    public ItemSlot[] slots;   // ���Ե� ��Ƴ������� �迭 �ε�


    public GameObject inventoryWindow;  //�κ��丮â
    public Transform dropPosition;  //����Ʈ���� ��ġ


    [Header("Selected Item")]
    private ItemSlot selectedItem;  // �����Ѿ����� ��� ����?
    private int selectedItemIndex;  // ������ ���� �ε���
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
    public UnityEvent onOpenInventory;  //���� �߻��ϴ� �̺�Ʈ
    public UnityEvent onCloseInventory; // ���� �� �߻��ϴ� �̺�Ʈ

    public static Inventory instance;  //�̱���?
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

        for (int i = 0; i < slots.Length; i++)  //������ �κ��丮�� ���� ��ȣ�� �ʱ�ȭ�� ������
        {
            slots[i] = new ItemSlot(); 
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        ClearSeletecItemWindow();
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext) //�κ��丮�� �Ѹ�?
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }


    public void Toggle() // �ִ� �����ϴ� �޼���
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
        return inventoryWindow.activeInHierarchy; //�θ������Ʈ�� ��츦 ����Ͽ� ������Ʈ�� ���¸� Ȯ���� ���
    }

    public void AddItem(ItemData item)
    {
        if (item.canStack) //�������� ���� �� �ִٸ�
        {
            ItemSlot slotToStackTo = GetItemStack(item); //������ �ױ�
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++; //������ �ױ�
                UpdateUI(); //���ε��ϱ�
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();  //�󽽷��� ã�� �޼���

        if (emptySlot != null) //������ �ִٸ� -> �̰� ������ �Ұ��Ѱ��
        {
            emptySlot.item = item; //������ �߰�
            emptySlot.quantity = 1; // 1
            UpdateUI();//���ε�
            return;
        }

        ThrowItem(item); //�Ѵپƴϸ� ���Ծ��
    }

    void ThrowItem(ItemData item)  //������ ������ �޼���
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) //�������� ������ ����
                uiSlots[i].Set(slots[i]);
            else
                uiSlots[i].Clear();  //�������� ������ Ŭ����
        }
    }

    ItemSlot GetItemStack(ItemData item)  //���ýױ�
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount) //�������� ������ �����鼭 �ִ�ġ�� ���� �ʴ´ٸ�
                return slots[i];
        }

        return null;
    }

    ItemSlot GetEmptySlot() //�󽽷� ã�Ƽ� �ֱ�
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

        selectedItem = slots[index]; //������ ã��
        selectedItemIndex = index;  // ���Թ�ȣ

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++) //���ٴ� �޼����ε�
        {
            selectedItemStatNames.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValues.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    private void ClearSeletecItemWindow()  //�����Ѿ����� �����
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

        void RemoveSelectedItem() //������ �����ϱ�
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
