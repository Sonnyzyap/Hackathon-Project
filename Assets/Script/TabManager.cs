using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabManager : MonoBehaviour
{
    public GameObject[] panels; // An array to hold the panels
    public Button[] buttons; // An array to hold the buttons

    private void Start()
    {
        // Initially, show only the first panel
        ShowPanel(0);
        
        // Assign the button click events
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Local copy of the index for the delegate
            buttons[i].onClick.AddListener(() => ShowPanel(index));
        }
    }

    public void ShowPanel(int index)
    {
        // Hide all panels
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        // Show the selected panel
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
        }
    }
}
