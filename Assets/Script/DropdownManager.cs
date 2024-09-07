using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_InputField inputField;
    public Button addButton;

    private List<string> dropdownOptions = new List<string>();

    void Start()
    {
        // Initialize the dropdown options with any pre-existing items
        dropdownOptions.AddRange(dropdown.options.ConvertAll(option => option.text));

        // Add a listener to the button
        addButton.onClick.AddListener(AddOptionToDropdown);
        dropdown.onValueChanged.AddListener(UpdateInputField);
    }

    void AddOptionToDropdown()
    {
        string newOption = inputField.text;

        if (!string.IsNullOrEmpty(newOption) && !dropdownOptions.Contains(newOption))
        {
            // Add the new option to the list
            dropdownOptions.Add(newOption);

            // Update the dropdown options
            dropdown.ClearOptions();
            dropdown.AddOptions(dropdownOptions);

            // Clear the input field after adding
            inputField.text = "";
        }
    }

    void UpdateInputField(int selectedIndex)
    {
        string originalText = inputField.text;

        // Get the selected option's text
        string selectedOption = dropdown.options[selectedIndex].text;

        // Update the input field with the selected option's text
        inputField.text = originalText + selectedOption;
    }
}