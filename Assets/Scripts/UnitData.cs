using System;

[Serializable]
public class UnitData
{
    public UnitType type;
    public CivilizationName owner;
    public float movePoints;
    public float currentMovePoints;
    public int sight;
    public bool isAttack;
    public float healthPoints;
    public float attack;
    public float defence;
    public int range;
}