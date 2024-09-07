using UnityEngine;
using UnityEngine.UI;

public class ErrorBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void OnButtonClick()
    {
        Destroy(gameObject);
    }
}
