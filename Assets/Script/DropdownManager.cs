using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_InputField inputField;
    public Button addButton;

    // Reference to the centralized template manager
    private CommentTemplateManager templateManager;

    void Start()
    {
        // Find the centralized template manager in the scene
        templateManager = FindObjectOfType<CommentTemplateManager>();

        // Populate the dropdown with the latest template list
        UpdateDropdownOptions();

        // Add a listener to the add button
        addButton.onClick.AddListener(AddOptionToDropdown);

        // Add a listener to update the input field when a template is selected
        dropdown.onValueChanged.AddListener(UpdateInputField);
    }

    // Update the dropdown options with the centralized template list
    public void UpdateDropdownOptions()
    {
        if (templateManager != null)
        {
            // Get the list of templates from the manager
            List<string> dropdownOptions = templateManager.GetTemplateList();

            // Update the dropdown options
            dropdown.ClearOptions();
            dropdown.AddOptions(dropdownOptions);
        }
    }

    // Add the input field text as a new template to the centralized list
    void AddOptionToDropdown()
    {
        string newOption = inputField.text;

        if (!string.IsNullOrEmpty(newOption) && templateManager != null)
        {
            // Add the new template to the centralized list
            templateManager.AddTemplate(newOption);

            // Clear the input field after adding
            inputField.text = "";

            // Update the dropdown with the new template list
            UpdateDropdownOptions();
        }
    }

    // Update the input field with the selected template
    void UpdateInputField(int selectedIndex)
    {
        string originalText = inputField.text;
        if (dropdown.options.Count > 0)
        {
            // Get the selected option's text
            string selectedOption = dropdown.options[selectedIndex].text;

            // Update the input field by appending the selected option's text
            inputField.text = originalText + selectedOption;
        }
    }
}
