using UnityEngine;

public class TimeConverter
{
    static int hours = 6;
    static int minutes = 0;

    // 숫자를 받아 시간을 추가하는 함수
    public static string AddTime(float value)
    {
        QuestlogUI.instance.UpdateSelected(value);

        // 입력된 숫자를 시간과 분으로 변환
        int additionalHours = Mathf.FloorToInt(value / 60);
        int additionalMinutes = Mathf.FloorToInt(value % 60);

        // 시간과 분을 더하기
        hours += additionalHours;
        minutes += additionalMinutes;

        if (hours >= 24)
        {
            hours = hours % 24;
            QuestlogUI.instance.QuestRenewal();
        }

        // 분이 60을 넘어가면 시간에 추가하고 분을 0으로 초기화
        if (minutes >= 60)
        {
            hours += Mathf.FloorToInt(minutes / 60);
            minutes = minutes % 60;
        }

        // 시간과 분을 문자열로 조합
        string timeString;

        if (minutes % 180 == 0)
            timeString = string.Format("{0:D2}:00", hours);
        else
            timeString = string.Format("{0:D2}:{1:D2}", hours, minutes);

        return timeString;
    }

    public static string CalculateTime(float value)
    {
        int days = (int)(value / 1440); // 1일 = 24시간 * 60분 = 1440분
        int hours = (int)((value % 1440) / 60);
        int minutes = (int)(value % 60);

        return $"{days}일 {hours}시간 {minutes}분";
    }
}