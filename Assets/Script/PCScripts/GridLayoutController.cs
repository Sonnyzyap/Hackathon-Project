using UnityEngine;
using UnityEngine.UI;

public class GridLayoutController : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public GameObject itemPrefab;
    public int numberOfItems = 10;

    void Start()
    {
        PopulateGrid();
    }

    void PopulateGrid()
    {
        // �O���b�h�ɃA�C�e����ǉ�
        for (int i = 0; i < numberOfItems; i++)
        {
            GameObject item = Instantiate(itemPrefab, gridLayoutGroup.transform);
            // �A�C�e���̐ݒ�������ōs��
        }
    }
}
