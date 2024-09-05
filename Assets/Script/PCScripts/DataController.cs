using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; //�l�b�g���[�N�p�̃l�[���X�y�[�X
using UnityEngine.UI;


public class DataController : MonoBehaviour
{
    [SerializeField] Text viewText;
    string url = "https://docs.google.com/spreadsheets/d/1xi7ljCNvzy9lssvxFl43BmZy3IQ42Vd0UMBS1UkNECs/gviz/tq?tqx=out:csv&sheet=�e�X�g�P";
    List<string> datas = new List<string>(); //�f�[�^�i�[�p��Stgring�^��List

    void Start()
    {
        StartCoroutine(GetData()); //�f�[�^�擾�p�̃R���[�`��
    }


    IEnumerator GetData()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url)) //UnityWebRequest�^�I�u�W�F�N�g
        {
            yield return req.SendWebRequest(); //URL�Ƀ��N�G�X�g�𑗂�

            if (IsWebRequestSuccessful(req)) //���������ꍇ
            {
                ParseData(req.downloadHandler.text); //�󂯎�����f�[�^�𐮌`����֐��ɏ���n��
                DisplayText(); //�f�[�^��\������
            }
            else                            //���s�����ꍇ
            {
                Debug.Log("error");
            }
        }
    }

    //�f�[�^�𐮌`����֐�
    void ParseData(string csvData)
    {
        string[] rows = csvData.Split(new[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries); //�X�v���b�h�V�[�g��1�s���z��Ɋi�[
        foreach (string row in rows)
        {
            string[] cells = row.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);//��s���̏���1�Z�����z��Ɋi�[
            foreach (string cell in cells)
            {
                string trimCell = cell.Trim('"'); //�Z���̕����񂩂�_�u���N�H�[�e�[�V����������
                if (!string.IsNullOrEmpty(trimCell)) //�������������񂪋󔒂łȂ����datas�ɒǉ����Ă���
                {
                    datas.Add(trimCell);
                }
            }
        }
    }

    //������\��������֐�
    void DisplayText()
    {
        foreach (string data in datas)
        {
            viewText.text += data + "\n";
        }
    }

    //���N�G�X�g�������������ǂ������肷��֐�
    bool IsWebRequestSuccessful(UnityWebRequest req)
    {
        /*�v���g�R���G���[�ƃR�l�N�g�G���[�ł͂Ȃ��ꍇ��true��Ԃ�*/
        return req.result != UnityWebRequest.Result.ProtocolError &&
               req.result != UnityWebRequest.Result.ConnectionError;
    }
}