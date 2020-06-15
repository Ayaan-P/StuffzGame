using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour
{
    // Start is called before the first frame update
    public string unitname;
    public int atk;
    public int spatk;
    public int def;
    public int spdef;
    public int spd;
    public int maxHP;
    public int currHP;

    public bool takeDamage(int enemyatk)
    {
        currHP -= (enemyatk-def);

        if(currHP<=0)
            return true;
        else
            return false;
    }
}
