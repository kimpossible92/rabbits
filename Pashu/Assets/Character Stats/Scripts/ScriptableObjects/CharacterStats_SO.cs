using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    [System.Serializable]
    public class CharLevelUps
    {
        public int maxHealth;
        public int maxMana;
        public int maxWealth;
        public int baseDamage;
        public float baseResistance;
        public float maxEncumbrance;
    }

    #region Fields
    public bool setManually = false;
    public bool saveDataOnClose = false;

    //public ItemPickUp weapon { get; private set; }
    //public ItemPickUp headArmor { get; private set; }
    //public ItemPickUp chestArmor { get; private set; }
    //public ItemPickUp handArmor { get; private set; }
    //public ItemPickUp legArmor { get; private set; }
    //public ItemPickUp footArmor { get; private set; }
    //public ItemPickUp misc1 { get; private set; }
    //public ItemPickUp misc2 { get; private set; }

    public int maxHealth = 0;
    public int currentHealth = 0;

    public int maxWealth = 0;
    public int currentWealth = 0;

    public int maxMana = 0;
    public int currentMana = 0;

    public int baseDamage = 0;
    public int currentDamage = 0;

    public float baseResistance = 0;
    public float currentResistance = 0f;

    public float maxEncumbrance = 0f;
    public float currentEncumbrance = 0f;

    public int charExperience = 0;
    public int charLevel = 0;

    public CharLevelUps[] charLevelUps;
    #endregion

    #region Stat Reducers
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeMana(int amount)
    {
        currentMana -= amount;

        if (currentMana < 0)
        {
            currentMana = 0;
        }
    }
    #endregion

    #region Character Level Up and Death
    private void Death()
    {
        Debug.Log("You kicked it! Sorry Moosa-Magoose.");
        //Call to Game Manager for Death State to trigger respawn
        //Dispaly the Death visualization
    }

    private void LevelUp()
    {
        charLevel += 1;
        //Display Level Up Visualization

        maxHealth = charLevelUps[charLevel -1].maxHealth;
        maxMana = charLevelUps[charLevel - 1].maxMana;
        maxWealth = charLevelUps[charLevel - 1].maxWealth;
        baseDamage = charLevelUps[charLevel - 1].baseDamage;
        baseResistance = charLevelUps[charLevel - 1].baseResistance;
        maxEncumbrance = charLevelUps[charLevel - 1].maxEncumbrance;
    }
    #endregion

}