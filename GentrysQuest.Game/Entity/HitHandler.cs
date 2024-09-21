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

using GentrysQuest.Game.Database;
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

    private DrawableEntity receiver;
    private Entity receiverBase;
    private Entity sender;
    private StatTracker stats;

    public HitHandler(Entity sender, DrawableEntity receiver)
    {
        Details = new DamageDetails();
        Details.Sender = this.sender = sender;
        this.receiver = receiver;
        Details.Receiver = receiverBase = receiver.GetBase();
        stats = GameData.CurrentStats;

        // logic
        calcDamage();
        applyDamage();
        applyHitCount();
        applyKnockback();
        invokeHitEvent();
        applyRewards();
    }

    private bool getCritChance() => sender.Stats.CritRate.Current.Value > MathBase.RandomInt(0, 100);

    private void calcDamage()
    {
        int damage = (int)(sender.Stats.Attack.GetCurrent() + sender.Weapon!.Damage.GetCurrent());
        Details.IsCrit = getCritChance();
        if (Details.IsCrit) damage += (int)MathBase.GetPercent(damage, sender.Stats.CritDamage.GetCurrent());
        Details.Damage = damage;
    }

    private void applyDamage()
    {
        if (Details.IsCrit) receiverBase.CritWithDefense(Details.Damage);
        else receiverBase.DamageWithDefense(Details.Damage);
        receiverBase.RemoveTenacity();
    }

    private void applyHitCount()
    {
        if (!sender.EnemyHitCounter.TryAdd(receiverBase, 1)) sender.EnemyHitCounter[receiverBase]++;
    }

    private void applyKnockback()
    {
        Vector2 direction = MathBase.GetDirection(sender.positionRef, receiver.Position);
        float knockbackForce = sender.Weapon!.KnockbackMultiplier / receiverBase.KnockbackModifier;
        if (Details.IsCrit) knockbackForce *= 1.5f;
        if (receiverBase.HasTenacity()) receiver.ApplyKnockback(direction, knockbackForce, (int)knockbackForce * 100, KnockbackType.StopsMovement);
        else receiver.ApplyKnockback(direction, knockbackForce, (int)knockbackForce * 200, KnockbackType.Stuns);
    }

    private void invokeHitEvent()
    {
        receiverBase.OnHit(Details);
        sender.Weapon!.HitEntity(Details);
    }

    private void applyRewards()
    {
        switch (receiverBase)
        {
            case Character character:
                stats.AddToStat(StatTypes.HitsTaken);
                if (Details.IsCrit) GameData.CurrentStats.AddToStat(StatTypes.CritsTaken);
                break;

            case Entity:
                stats.AddToStat(StatTypes.Hits);
                break;
        }

        switch (sender)
        {
            case Character character:
                if (receiverBase.IsDead)
                {
                    GameData.CurrentStats.AddToStat(StatTypes.Hits);
                    if (Details.IsCrit) GameData.CurrentStats.AddToStat(StatTypes.Crits);
                    GameData.CurrentStats.AddToStat(StatTypes.Damage, Details.Damage);
                    GameData.CurrentStats.AddToStat(StatTypes.MostDamage, Details.Damage);
                    int money = receiverBase.GetMoneyReward();
                    GameData.CurrentStats.AddToStat(StatTypes.MoneyGained, money);
                    GameData.CurrentStats.AddToStat(StatTypes.MoneyGainedOnce, money);
                    sender.AddXp(receiverBase.GetXpReward());
                    GameData.Money.Hand(money);

                    Weapon.Weapon reward = receiverBase.GetWeaponReward();
                    if (reward != null) GameData.Add(reward);

                    GameData.CurrentStats.AddToStat(StatTypes.Kills);
                }

                break;
        }
    }
}
