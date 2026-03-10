// * Name              : GentrysQuest.Game
//  * Author           : Brayden J Messerschmidt
//  * Created          : 07/29/2024
//  * Course           : CIS 169 C#
//  * Version          : 1.0
//  * OS               : Windows 11 22H2
//  * IDE              : Jet Brains Rider 2023
//  * Copyright        : This is my work.
//  * Description      : desc.
//  * Academic Honesty : I attest that this is my original work.
//  * I have not used unauthorized source code, either modified or
//  * unmodified. I have not given other fellow student(s) access
//  * to my program.

using System.Collections.Generic;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osuTK;

namespace GentrysQuest.Game.Entity;

public class HitHandler
{
    /// <summary>
    /// The details of the hit
    /// </summary>
    public DamageDetails Details { get; private set; }

    private readonly DrawableEntity receiver;
    private readonly Entity receiverBase;
    private readonly Entity sender;

    public HitHandler(Entity sender, DrawableEntity receiver, List<StatusEffect> statusEffects = null, int projectileDamage = 0)
    {
        Details = new DamageDetails();
        if (statusEffects != null) Details.StatusEffects = statusEffects;
        Details.Sender = this.sender = sender;
        this.receiver = receiver;
        Details.Receiver = receiverBase = receiver.GetBase();

        // logic
        calcDamage(projectileDamage);
        invokeHitEvent();
        applyHitCount();
        applyKnockback();
        applyDamage();
        applyRewards();
    }

    private bool getCritChance() => sender.Stats.CritRate.Current.Value > MathBase.RandomInt(0, 100);

    private void calcDamage(int projectileDamage)
    {
        int weaponDamage = 0;
        if (sender.Weapon != null) weaponDamage = (int)sender.Weapon.Damage.GetCurrent();

        int damage = (int)(sender.Stats.Attack.GetCurrent() + weaponDamage);
        damage += projectileDamage;
        Details.IsCrit = getCritChance();
        if (Details.IsCrit) damage += (int)MathBase.GetPercent(damage, sender.Stats.CritDamage.GetCurrent());
        Details.Damage = damage;
    }

    private void applyDamage()
    {
        if (Details.IgnoreDefense)
        {
            if (Details.IsCrit) receiverBase.Crit(Details.Damage);
            else receiverBase.Damage(Details.Damage);
        }
        else
        {
            if (Details.IsCrit) receiverBase.CritWithDefense(Details.Damage);
            else receiverBase.DamageWithDefense(Details.Damage);
        }

        receiverBase.RemoveTenacity();
    }

    private void applyHitCount()
    {
        if (!sender.EnemyHitCounter.TryAdd(receiverBase, 1)) sender.EnemyHitCounter[receiverBase]++;
    }

    private void applyKnockback()
    {
        if (sender.Weapon == null) return;

        Vector2 direction = MathBase.GetDirection(sender.PositionRef, receiver.Position);
        float knockbackForce = sender.Weapon!.KnockbackMultiplier / receiverBase.KnockbackModifier;
        if (Details.IsCrit) knockbackForce *= 1.5f;
        if (receiverBase.HasTenacity()) receiver.ApplyKnockback(direction, knockbackForce, (int)knockbackForce * 100, KnockbackType.StopsMovement);
        else receiver.ApplyKnockback(direction, knockbackForce, (int)knockbackForce * 200, KnockbackType.Stuns);
    }

    private void invokeHitEvent()
    {
        receiverBase.OnHit(Details);
        foreach (var effect in Details.StatusEffects) receiverBase.AddEffect(effect);
        if (sender.Weapon != null) sender.Weapon!.HitEntity(Details);
    }

    private void applyRewards()
    {
        switch (receiverBase)
        {
            case Character character:
                break;

            case Entity:
                break;
        }

        switch (sender)
        {
            case Character character:
                if (receiverBase.IsDead)
                {
                    int money = receiverBase.GetMoneyReward();
                    sender.AddXp(receiverBase.GetXpReward());
                    Weapon.Weapon reward = receiverBase.GetWeaponReward();
                }

                break;
        }
    }
}
