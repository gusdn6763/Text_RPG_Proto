[System.Serializable]
public class Status
{
    public float hp;
    public float fatigue;
    public float hungry;
    public float time;
    public Status()
    {
        fatigue = 0;
        hungry = 0;
        time = 0;
    }

    public Status(float hp, float fatigue, float hungry, float time)
    {
        this.hp = hp;
        this.fatigue = fatigue;
        this.hungry = hungry;
        this.time = time;
    }

    public static Status operator +(Status stat1, Status stat2)
    {
        return new Status(
            stat1.hp + stat2.hp,
            stat1.fatigue + stat2.fatigue,
            stat1.hungry + stat2.hungry,
            stat1.time + stat2.time
        );
    }
    public static Status operator -(Status stat1, Status stat2)
    {
        return new Status(
            stat1.hp - stat2.hp,
            stat1.fatigue - stat2.fatigue,
            stat1.hungry - stat2.hungry,
            stat1.time - stat2.time
        );
    }
}