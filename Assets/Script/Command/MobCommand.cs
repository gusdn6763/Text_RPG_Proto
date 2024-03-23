using UnityEngine;

public class MobCommand : Command
{
    [SerializeField] private string corpseName;

    public void Die()
    {
        if (Player.instance.monsters.Contains(col))
        {
            Player.instance.monsters.Remove(col);
            if (Player.instance.monsters.Count == 0)
                Player.instance.CombatOn = false;
        }

        CommandName = corpseName;
        CommandActive(Constant.attack, false);
        CommandActive(Constant.looting, true);
        CommandActive(Constant.delete, true);
    }
}
