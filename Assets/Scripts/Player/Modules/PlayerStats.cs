// Class that holds all the upgradable stats for the player character
// Each of the stats are translated differently into in-game values

// Vitality: A 1 point increase in vitality results in a 2.5% increase in max health
// Might: A 1 point increase in might results in a 1 point increase in base damage
// Perception: 

public class PlayerStats
{
    public static int vitality = 0;
    public static int might = 0;
    public static int perception = 0;

    public delegate void PlayerStatChange(int statTotal);
    public static PlayerStatChange vitalityIncrease;
    public static PlayerStatChange mightIncrease;
    public static PlayerStatChange perceptionIncrease;

    public static string[] GetStats()
    {
        string[] returnVal = {
            (vitality + 1).ToString(),
            (might + 1).ToString(),
            (perception + 1).ToString(),
        };
        return returnVal;
    }
    

    public static void IncreaseVitality(int amount) {
        vitality += amount;
        vitalityIncrease(vitality);
    }

    public static void IncreaseMight(int amount) {
        might += amount;
        mightIncrease(might);
    }

    public static void IncreasePerception(int amount) {
        perception += amount;
        perceptionIncrease(perception);
    }

}