using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    private GameObject _nearObject;
    private ItemObject _nearItemObject;
    public GameObject press;
    public TextMeshProUGUI promptText;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {
            _nearObject = other.gameObject;
            _nearItemObject = _nearObject.GetComponent<ItemObject>();
            SetPromptText();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            _nearObject = null;
            _nearItemObject = null;
            OffPromptText();
        }
    }

    public void SetPromptText()
    {
        press.gameObject.SetActive(true);
        promptText.text = _nearItemObject.item.displayName;
    }

    public void OffPromptText()
    {
        press.gameObject.SetActive(false);
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (_nearObject != null && context.phase == InputActionPhase.Started)
        {
            _nearItemObject.OnInteract();
            Destroy(_nearObject);
            press.gameObject.SetActive(false);
        }
    }
}


