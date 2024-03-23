using UnityEngine;

public class TimeConverter
{
    static int hours = 6;
    static int minutes = 0;

    // ���ڸ� �޾� �ð��� �߰��ϴ� �Լ�
    public static string AddTime(float value)
    {
        QuestlogUI.instance.UpdateSelected(value);

        // �Էµ� ���ڸ� �ð��� ������ ��ȯ
        int additionalHours = Mathf.FloorToInt(value / 60);
        int additionalMinutes = Mathf.FloorToInt(value % 60);

        // �ð��� ���� ���ϱ�
        hours += additionalHours;
        minutes += additionalMinutes;

        if (hours >= 24)
        {
            hours = hours % 24;
            QuestlogUI.instance.QuestRenewal();
        }

        // ���� 60�� �Ѿ�� �ð��� �߰��ϰ� ���� 0���� �ʱ�ȭ
        if (minutes >= 60)
        {
            hours += Mathf.FloorToInt(minutes / 60);
            minutes = minutes % 60;
        }

        // �ð��� ���� ���ڿ��� ����
        string timeString;

        if (minutes % 180 == 0)
            timeString = string.Format("{0:D2}:00", hours);
        else
            timeString = string.Format("{0:D2}:{1:D2}", hours, minutes);

        return timeString;
    }

    public static string CalculateTime(float value)
    {
        int days = (int)(value / 1440); // 1�� = 24�ð� * 60�� = 1440��
        int hours = (int)((value % 1440) / 60);
        int minutes = (int)(value % 60);

        return $"{days}�� {hours}�ð� {minutes}��";
    }
}