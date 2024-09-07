using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSceneSwitcher : MonoBehaviour
{
    public string sceneName; // �J�ڐ�̃V�[����

    void Start()
    {
        // ���̃X�N���v�g���A�^�b�`����Ă���{�^�����擾
        Button button = GetComponent<Button>();

        //// �{�^���̃N���b�N�C�x���g�Ƀ��\�b�h��o�^
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // �w�肳�ꂽ�V�[���ɐ؂�ւ���
        SceneManager.LoadScene(sceneName);
    }
}
