using System.Collections.Generic;
using UnityEngine;


public static class CardDatabase
{
    public static List<SkillCard> GetAllCards()
    {
        return new List<SkillCard>
        {
            new SkillCard("�����K��",  "�I����ܤ@���m������]���ת��a�q�L�^", false, new Color(1f, 0.4f, 0.4f)), // ����
            new SkillCard("���u", "�Ѥl�ܳ���", false, new Color(1f, 0.8f, 0.2f)),   // ����
            new SkillCard("�蹳�]�k", "�Ϥ�V����", true, new Color(0.6f, 0.8f, 1f))  // �Ŧ�

        };
    }
}
