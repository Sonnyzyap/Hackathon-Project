using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Linq;

public class CalenderButtonController : MonoBehaviour
{
    private Button buttonToday; // �uButton_Today�v�{�^�����i�[����ϐ�

    void Start()
    {
        // ���̃X�N���v�g���A�^�b�`����Ă���{�^�����擾
        Button button = GetComponent<Button>();

        // �V�[��������uButton_Today�v�I�u�W�F�N�g���擾���AButton�R���|�[�l���g���擾
        GameObject buttonObject = GameObject.Find("Button_Today");
        if (buttonObject != null)
        {
            buttonToday = buttonObject.GetComponent<Button>();
        }
        else
        {
            Debug.LogError("Button_Today ���V�[�����Ɍ�����܂���ł����B");
        }

        // �{�^���̃N���b�N�C�x���g�Ƀ��\�b�h��o�^
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // ���̃X�N���v�g���A�^�b�`����Ă���{�^����Text�R���|�[�l���g���擾
        Text buttonText = GetComponentInChildren<Text>();

        // �uButton_Today�v�{�^����Text�R���|�[�l���g���擾
        Text anotherButtonText = buttonToday?.GetComponentInChildren<Text>();

        // �e�L�X�g�̓��e���Ȃ��ăR���\�[���ɏo��
        if (buttonText != null && anotherButtonText != null)
        {
            string combinedText = anotherButtonText.text + buttonText.text;
            Debug.Log(TraverseString(combinedText));
        }
        else
        {
            Debug.Log("�e�L�X�g�R���|�[�l���g��������܂���ł����B");
        }
    }

    static string TraverseString(string input)
    {
        StringBuilder sb = new StringBuilder(input);

        // for���[�v�ŕ�����𑖍�
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c == '��')
            {
                sb[i] = '-';  // StringBuilder�ŕ�����u��
            }
            else if (c == '�N')
            {
                sb[i] = '-';  // StringBuilder�ŕ�����u��
            }
        }

        return sb.ToString();  // �u����̕������Ԃ�
    }

}


