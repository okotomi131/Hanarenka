/*
 ===================
 ��Ր���F����
 �ǋL�F���E�q��
 ===================
 */

using UnityEngine;
using UnityEngine.UI; // UI�R���|�[�l���g�̎g�p

public class ButtonSelected : MonoBehaviour
{
    private Button UISelect;
    void Start()
    {
        UISelect = GetComponent<Button>();
        // �{�^���I�������Ȃ�Ȃ��悤�ɂ���
        GetComponent<ButtonAnime>().bPermissionSelectSE = false;
        // �ŏ��ɑI����Ԃɂ������{�^���̐ݒ�
        UISelect.Select();
    }
}
