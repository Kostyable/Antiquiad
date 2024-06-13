public class CombatUnit : Unit
{
    public float healthPoints;
    public float attack;
    public float defence;

    public override void Init(UnitData data, Cell cell, Civilization owner)
    {
        healthPoints = data.healthPoints;
        attack = data.attack;
        defence = data.defence;
        base.Init(data, cell, owner);
    }
    
    public virtual bool Attack(Unit unit)
    {
        return false;
    }
    
    public virtual bool Attack(City city)
    {
        return false;
    }

    public void Damagable(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            healthPoints = 0;
            Destroy(gameObject);
        }
    }
}