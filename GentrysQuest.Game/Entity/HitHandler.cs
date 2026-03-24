using System.Collections.Generic;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
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
        string damageText = "0";
        ColourInfo damageDisplay = ColourInfo.SingleColour(Colour4.White);

        if (Details.IsCrit)
        {
            Details.Damage += (int)MathBase.GetPercent(Details.Damage, sender.Stats.CritDamage.GetCurrent());
            damageDisplay = ColourInfo.SingleColour(Colour4.Red);
        }

        if (!Details.IgnoreDefense) Details.Damage = receiverBase.AfterDefense(Details.Damage);

        switch (receiverBase.Invincible)
        {
            case true:
                damageText = "Missed";
                damageDisplay = ColourInfo.SingleColour(Colour4.Gray);
                break;

            default:
            {
                if (receiverBase.IsDodging)
                {
                    damageText = "Dodged";
                    damageDisplay = ColourInfo.SingleColour(Colour4.Gray);
                }
                else
                {
                    receiverBase.Damage(Details);
                    receiverBase.RemoveTenacity();
                    damageText = Details.Damage.ToString();
                }

                break;
            }
        }

        receiverBase.DisplayHealthEvent(damageText, damageDisplay);
    }

    private void applyHitCount()
    {
        if (!sender.HitCounter.TryAdd(receiverBase, 1)) sender.HitCounter[receiverBase]++;
    }

    private void applyKnockback()
    {
        if (sender.Weapon == null) return;
        if (Details.WasDodged) return;
        if (receiverBase.Invincible) return;

        Vector2 direction = MathBase.GetDirection(sender.PositionRef, receiver.Position);
        float knockbackForce = sender.Weapon!.KnockbackMultiplier / receiverBase.KnockbackModifier;
        if (Details.IsCrit) knockbackForce *= 1.5f;
        if (receiverBase.HasTenacity()) receiver.ApplyKnockback(direction, knockbackForce, (int)knockbackForce * 100, KnockbackType.StopsMovement);
        else receiver.ApplyKnockback(direction, knockbackForce, (int)knockbackForce * 200, KnockbackType.Stuns);
    }

    private void invokeHitEvent()
    {
        receiverBase.OnHit(Details);
        if (receiverBase.IsDodging) Details.WasDodged = true;
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
                if (receiverBase.IsDead) sender.AddXp(receiverBase.GetXpReward());

                break;
        }
    }
}
