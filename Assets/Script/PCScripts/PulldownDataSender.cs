using UnityEngine;
using TMPro; // TextMeshProUGUI ���g�����߂̖��O���
using UnityEngine.UI;

public class PulldownDataSender : MonoBehaviour
{
    // Dropdown�R���|�[�l���g���C���X�y�N�^���犄�蓖�Ă�
    public TMP_Dropdown dropdown;
    public string selectedText;
    public SendGraphData sdg;

    void Start()
    {
        // Dropdown�R���|�[�l���g���ݒ肳��Ă��邩�m�F
        if (dropdown != null)
        {
            // Dropdown�̑I��ύX�C�x���g�Ƀ��\�b�h��o�^
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            sdg = new SendGraphData();
        }
        else
        {
            Debug.LogError("Dropdown �R���|�[�l���g���C���X�y�N�^����ݒ肳��Ă��܂���B");
        }
    }

    void OnDropdownValueChanged(int index)
    {
        // �I�����ꂽ���ڂ̃e�L�X�g���擾
        if (dropdown != null && dropdown.options.Count > index)
        {
            selectedText = dropdown.options[index].text;
            if (selectedText == "�T")
            {
                sdg.timeLine = "week";
            }
            else if (selectedText == "��")
            {
                sdg.timeLine = "month";
            }
            else if (selectedText == "�N")
            {
                sdg.timeLine = "year";
            }
            else if (selectedText == "�N����")
            {
                sdg.timeLine = "byYear";
            }
            else if (selectedText == "1�n")
            {
                selectedText = "1series";
            }
            else if (selectedText == "2�n")
            {
                selectedText = "2series";
            }
            else if (selectedText == "3�n-1")
            {
                selectedText = "3series1";
            }
            else if (selectedText == "3�n-2")
            {
                selectedText = "3series2";
            }
            else if (selectedText == "�S��")
            {
                selectedText = "allSeries";
            }
            Debug.Log(selectedText);
        }
        else
        {
            Debug.LogError("�I�����ꂽ���ڂ�������܂���ł����B");
        }
    }
}
