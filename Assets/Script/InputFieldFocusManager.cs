using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // For handling input events

public class InputFieldFocusManager : MonoBehaviour
{
    public TMP_InputField[] inputFields; // Assign your input fields in the Inspector

    private void Start()
    {
        // Add event listeners to each input field
        for (int i = 0; i < inputFields.Length; i++)
        {
            int index = i; // Local copy for the lambda expression
            inputFields[i].onEndEdit.AddListener((text) => OnEndEdit(index));
            // Optionally, add listener for "Submit" if you need special handling
            inputFields[i].onSubmit.AddListener((text) => OnSubmit(index));
        }
    }

    private void OnEndEdit(int currentIndex)
    {
        // Automatically focus the next field if needed
        FocusNextField(currentIndex);
    }

    private void OnSubmit(int currentIndex)
    {
        // Optionally handle "Submit" event
        FocusNextField(currentIndex);
    }

    private void FocusNextField(int currentIndex)
    {
        if (currentIndex < inputFields.Length - 1)
        {
            int nextIndex = currentIndex + 1;
            inputFields[nextIndex].Select(); // Focus the next input field
            inputFields[nextIndex].ActivateInputField(); // Activate the field to ensure the keyboard remains open
        }
        else
        {
            // If it's the last input field, disable keyboard
            inputFields[currentIndex].DeactivateInputField(); // Optionally handle input field deactivation
            // Close the keyboard (if needed)
            TouchScreenKeyboard.hideInput = true; // Hide keyboard after last field
        }
    }
}
