[System.Serializable]
public class Stat
{
    public float health;        // °Ç°­
    public float strength;      // Èû
    public float intellect;     // Áö´É
    public float agility;       // ¹ÎÃ¸
    public Stat(float health, float strength, float intellect, float agility)
    {
        this.health = health;
        this.strength = strength;
        this.intellect = intellect;
        this.agility = agility;
    }

    public static Stat operator +(Stat stat1, Stat stat2)
    {
        return new Stat(
            stat1.health + stat2.health,
            stat1.strength + stat2.strength,
            stat1.intellect + stat2.intellect,
            stat1.agility + stat2.agility
        );
    }
    public static Stat operator -(Stat stat1, Stat stat2)
    {
        return new Stat(
            stat1.health - stat2.health,
            stat1.strength - stat2.strength,
            stat1.intellect - stat2.intellect,
            stat1.agility - stat2.agility
        );
    }
    public static bool operator >(Stat stat1, Stat stat2)
    {
        return stat1.health > stat2.health &&
               stat1.strength > stat2.strength &&
               stat1.intellect > stat2.intellect &&
               stat1.agility > stat2.agility;
    }
    public static bool operator <(Stat stat1, Stat stat2)
    {
        return stat1.health < stat2.health &&
               stat1.strength < stat2.strength &&
               stat1.intellect < stat2.intellect &&
               stat1.agility < stat2.agility;
    }
}