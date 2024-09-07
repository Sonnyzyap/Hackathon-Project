using System.Collections.Generic;
using UnityEngine;

public class CommentTemplateManager : MonoBehaviour
{
    private const string TemplatesKey = "CommentTemplates";

    // Centralized list of templates
    private List<string> commentTemplates = new List<string>();

    void Start()
    {
        LoadTemplates();
        UpdateAllDropdowns();
    }

    // Add a new template to the list
    public void AddTemplate(string newTemplate)
    {
        if (!string.IsNullOrEmpty(newTemplate) && !commentTemplates.Contains(newTemplate))
        {
            commentTemplates.Add(newTemplate);
            SaveTemplates();
            // Optionally update all dropdowns in the scene
            UpdateAllDropdowns();
        }
    }

    // Get the list of templates for the dropdowns
    public List<string> GetTemplateList()
    {
        return commentTemplates;
    }

    // Save the templates to PlayerPrefs
    private void SaveTemplates()
    {
        string json = JsonUtility.ToJson(new TemplateListWrapper { templates = commentTemplates });
        PlayerPrefs.SetString(TemplatesKey, json);
        PlayerPrefs.Save();
    }

    // Load the templates from PlayerPrefs
    private void LoadTemplates()
    {
        if (PlayerPrefs.HasKey(TemplatesKey))
        {
            string json = PlayerPrefs.GetString(TemplatesKey);
            TemplateListWrapper wrapper = JsonUtility.FromJson<TemplateListWrapper>(json);
            commentTemplates = wrapper.templates;
        }
        else
        {
            commentTemplates = new List<string>();
        }
    }

    // Optionally update all dropdowns when a new template is added
    public void UpdateAllDropdowns()
    {
        DropdownManager[] dropdowns = FindObjectsOfType<DropdownManager>();
        foreach (DropdownManager dropdown in dropdowns)
        {
            dropdown.UpdateDropdownOptions();
        }
    }

    // Helper class for serializing the list of templates
    [System.Serializable]
    private class TemplateListWrapper
    {
        public List<string> templates;
    }
}
