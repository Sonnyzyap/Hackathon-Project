using UnityEngine;
using TMPro; // TextMeshProUGUI ���g�����߂̖��O���
using UnityEngine.UI;

public class ButtonDataSender : MonoBehaviour
{
    void Start()
    {
        // ���̃X�N���v�g���A�^�b�`����Ă���{�^�����擾
        Button button = GetComponent<Button>();

        // �{�^���̃N���b�N�C�x���g�Ƀ��\�b�h��o�^
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // ���̃X�N���v�g���A�^�b�`����Ă���{�^����TextMeshProUGUI�R���|�[�l���g���擾
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // TextMeshProUGUI�R���|�[�l���g�����݂��邩�m�F
        if (buttonText != null)
        {
            string subButtonText = buttonText.text;
            if (subButtonText == "ph��")
            {
                subButtonText = "phEtc";
            }
            else if (subButtonText == "NH4��")
            {
                buttonText.text = "NH4Etc";
            }
            else if (subButtonText == "����")
            {
                subButtonText = "growth";
            }
            else if (subButtonText == "�R�����g")
            {
                subButtonText = "comments";
            }
            else if (subButtonText == "OK")
            {
                subButtonText = "phEtc";
            }
            else if (subButtonText == "�߂�")
            {
                subButtonText = "phEtc";
            }
            Debug.Log(subButtonText);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI�R���|�[�l���g��������܂���ł����B�{�^����TextMeshProUGUI�R���|�[�l���g���������A�^�b�`����Ă��邩�m�F���Ă��������B");
        }
    }
}
