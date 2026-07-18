using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TextInputField : UIComponent
{
    [SerializeField] TMP_InputField inputField;
    
    [SerializeField] Sprite defaultHighlightSprite;
    [SerializeField] Sprite activatedHighlightSprite;
    
    [SerializeField] bool storeInput = true;
    
    public UnityEvent<string> onSubmit;
    
    
    bool activated = false;

    void Awake()
    {
        onLeftClicked.AddListener(Activate); ;
        inputField.onSelect.AddListener(Select);
        inputField.onDeselect.AddListener(Deselect);
        onHoverStop.AddListener(Deactivate);
        inputField.onSubmit.AddListener(Submit);
    }

    private void OnDestroy()
    {
        onLeftClicked.RemoveListener(Activate);
        inputField.onSelect.RemoveListener(Select);
        inputField.onDeselect.RemoveListener(Deselect);
        inputField.onSubmit.RemoveListener(Submit);
        onHoverStop.RemoveListener(Deactivate);
    }

    public void Select(string text)
    {
        Activate();
    }

    public void Deselect(string text)
    {
        Deactivate();
    }

    public void Activate()
    {
        Debug.Log("TextInputField - Activated");
        inputField.ActivateInputField();
        activated = true;
        highlight.sprite = activatedHighlightSprite;
        highlight.enabled = true;
    }

    public void Deactivate()
    {
        Debug.Log("TextInputField - Deactivated");
        inputField.DeactivateInputField();
        activated = false;
        highlight.sprite = defaultHighlightSprite;
        highlight.enabled = false;
        if (!storeInput)
        {
            inputField.text = "";
        }
    }

    public void Submit(string currentText)
    {
        Debug.Log("TextInputField - Submitted: " + currentText);
        onSubmit?.Invoke(currentText);
        Deactivate();
    }
    
    public bool IsActive()
    {
        return activated;
    }
}
