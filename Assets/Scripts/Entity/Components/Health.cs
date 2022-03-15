using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
[System.Serializable]
public class Health
{
    public event Action OnDeath = delegate{};
    public event Action<int, int> OnDamageTaken = delegate{};
    public event Action OnHealTaken = delegate{};
    public int health;
    public int maxHealth;
    public Entity tiedEntity;
    public bool canBeAttacked = true;
    public bool spawnHealthBar = true;
    public bool canKnockback;
    Coroutine flashing;
    public int current {
        get { return health; }
        set {
            SetValue(value);
            if (IsDead) {
                OnDeath();            
            }
        }
    }
    public Health(int hp, Entity entity)
    {
        maxHealth = hp;
        health = maxHealth;
        tiedEntity = entity;
    }
    private void SetValue(int value){
        if(value > maxHealth){
            health = maxHealth;
        }
        else if(value < 0){
            health = 0;
        }
        else{
            health = value;
        }
    }

    public bool IsDead => current <= 0;
    public bool isDead(){
        return current <= 0;
    }
    public void DoDamage(int damage){
        current -= damage;
        OnDamageTaken(current, damage);
    }
    IEnumerator FlashingProtector(float timer){
        yield return new WaitForSeconds(timer);
        flashing = null;
    }
}
