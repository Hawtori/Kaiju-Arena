public class PlayerEnums
{
    private enum playerAttack
    {
        none,    // 0
        normal,  // 1
        ranged,  // 2
        special, // 3
        block    // 4
    }

    #region modes
        private int NONE = (int)playerAttack.none;
        private int NORMAL = (int)playerAttack.normal;
        private int RANGED = (int)playerAttack.ranged;
        private int SPECIAL = (int)playerAttack.special;
        private int BLOCK = (int)playerAttack.block;
        private int mode;
    #endregion

    #region attack distances
        private float normalDistance = 1f;
        private float rangedDistance = 5;
        private float specialDistance = 1;
    #endregion

    #region attack damage
        private float normalDamage = 1;
        private float rangedDamage = 0.5f;
        private float specialDamage = 1.5f;
    #endregion

    #region attack stamina
    private float normalStamina = 15f;
    private float rangedStamina = 20f;
    private float specialStamina = 30f;
    private float blockStamina = 10f;
    #endregion

    public void setMode(int m)
    {
        mode = m;
    }

    public int getMode()
    {
        return mode;
    }

    public float getDistance()
    {
        if (mode == 1) return normalDistance;
        else if (mode == 2) return rangedDistance;
        else return specialDistance;
    }

    public float getDamage()
    {
        if (mode == 1) return normalDamage;
        else if (mode == 2) return rangedDamage;
        else return specialDamage;
    }

    public float getStamina()
    {
        if (mode == 1) return normalStamina;
        else if (mode == 2) return rangedStamina;
        else if (mode == 3) return specialStamina;
        else return blockStamina;
    }

}