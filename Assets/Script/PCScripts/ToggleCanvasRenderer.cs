using UnityEngine;
using UnityEngine.UI;

public class ToggleCanvasRenderer : MonoBehaviour
{
    public Button toggleButton;     // �\��/��\����؂�ւ���{�^��
    public GameObject targetUIElement; // �\��/��\����؂�ւ�����UI�v�f

    void Start()
    {
        // �{�^�����ݒ肳��Ă��邩�m�F���A�ݒ肳��Ă���΃N���b�N�C�x���g��ǉ�
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleUIElement);
        }

        // UI�v�f���ݒ肳��Ă���Ώ�����Ԃ��\���ɂ���
        if (targetUIElement != null)
        {
            targetUIElement.SetActive(false);
        }
    }

    void ToggleUIElement()
    {
        // UI�v�f�̕\��/��\����؂�ւ���
        if (targetUIElement != null)
        {
            targetUIElement.SetActive(!targetUIElement.activeSelf);
        }
    }
}
