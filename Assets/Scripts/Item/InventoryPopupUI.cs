using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupUI : MonoBehaviour
{
    [Header("Confirmation Popup")]
    [SerializeField] private GameObject confirmationPopupObject;
    [SerializeField] private Text confirmationItemNameText;
    [SerializeField] private Text confirmationText;
    [SerializeField] private Button confirmationOKButton;
    [SerializeField] private Button confirmationCancelButton;

    [Header("Amount Input Popup")]
    [SerializeField] private GameObject amountInputPopupObject;
    [SerializeField] private Text amountInputItemNameText;
    [SerializeField] private InputField amountInputField;
    [SerializeField] private Button amountPlusButton;
    [SerializeField] private Button amountMinusButton;
    [SerializeField] private Button amountInputOKButton;
    [SerializeField] private Button amountInputCancelButton;

    private event Action OnConfirmationOK;
    private event Action<int> OnAmountInputOK;

    private int maxAmount;

    private void Awake()
    {
        InitUIEvents();
        HidePanel();
        HideConfirmationPopup();
        HideAmountInputPopup();




        
    }

    private void Update()
    {
        if (confirmationPopupObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                confirmationOKButton.onClick?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                confirmationCancelButton.onClick?.Invoke();
            }
        }
        else if(amountInputPopupObject.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                amountInputOKButton.onClick?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                amountInputCancelButton.onClick?.Invoke();
            }
        }
    }

    public void OpenConfirmationPopup(Action okCallback, string itemName)
    {
        ShowPanel();
        ShowConfirmationPopup(itemName);
        SetConfirmationOKEvent(okCallback);
    }

    public void OpenAmountInputPopup(Action<int> okCallback, int currentAmount, string itemName)
    {
        maxAmount = currentAmount - 1;
        amountInputField.text = "1";

        ShowPanel();
        ShowAmountInputPopup(itemName);
        SetAmountInputOKEvent(okCallback);
    }

    private void InitUIEvents()
    {
        confirmationOKButton.onClick.AddListener(HidePanel);
        confirmationOKButton.onClick.AddListener(HideConfirmationPopup);
        confirmationOKButton.onClick.AddListener(() => OnConfirmationOK?.Invoke());

        amountInputOKButton.onClick.AddListener(HidePanel);
        amountInputOKButton.onClick.AddListener(HideAmountInputPopup);

        amountMinusButton.onClick.AddListener(() =>
        {
            int.TryParse(amountInputField.text, out int amount);
            if (amount > 1)
            {
                int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount - 10 : amount - 1;
                if (nextAmount < 1)
                    nextAmount = 1;
                amountInputField.text = nextAmount.ToString();
            }
        });

        amountPlusButton.onClick.AddListener(() =>
        {
            int.TryParse(amountInputField.text, out int amount);
            if (amount < maxAmount)
            {
                int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount + 10 : amount + 1;
                if (nextAmount > maxAmount)
                    nextAmount = maxAmount;
                amountInputField.text = nextAmount.ToString();
            }
        });

        amountInputField.onValueChanged.AddListener(str =>
        {
            int.TryParse(str, out int amount);
            bool flag = false;

            if (amount < 1)
            {
                flag = true;
                amount = 1;
            }
            else if (amount > maxAmount)
            {
                flag = true;
                amount = maxAmount;
            }

            if (flag)
                amountInputField.text = amount.ToString();
        });
    }

    private void ShowPanel() => gameObject.SetActive(true);
    private void HidePanel() => gameObject.SetActive(false);

    private void ShowConfirmationPopup(string itemName)
    {
        confirmationItemNameText.text = itemName;
        confirmationPopupObject.SetActive(true);
    }
    private void HideConfirmationPopup() => confirmationPopupObject.SetActive(false);

    private void ShowAmountInputPopup(string itemName)
    {
        amountInputItemNameText.text = itemName;
        amountInputPopupObject.SetActive(true);
    }

    private void HideAmountInputPopup() => amountInputPopupObject.SetActive(false);
    private void SetConfirmationOKEvent(Action handler) => OnConfirmationOK = handler;
    private void SetAmountInputOKEvent(Action<int> handler) => OnAmountInputOK = handler;
}
