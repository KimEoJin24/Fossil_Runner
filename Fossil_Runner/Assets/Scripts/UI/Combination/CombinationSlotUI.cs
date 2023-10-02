using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombinationSlotUI : MonoBehaviour
{
    public int index;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    private ItemSlot curSlot;
    private Outline outline;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnButtonClick()
    {
        Combination.instance.SelectItem(index);
    }
}
