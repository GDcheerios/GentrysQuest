using System;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Entity;

public abstract class StatusEffect
{
    protected static int Identifier = 0;
    public int ID;

    protected StatusEffect(int duration = 1, int stack = 1)
    {
        Identifier++;
        ID = Identifier;

        Duration = duration;
        Stack = stack;
    }

    /// <summary>
    /// The name of the Effect duh
    /// </summary>
    public abstract string Name { get; set; }

    /// <summary>
    /// Description of the effect
    /// </summary>
    public abstract string Description { get; set; }

    /// <summary>
    /// The color for this Effect.
    /// </summary>
    public abstract Colour4 EffectColor { get; protected set; }

    /// <summary>
    /// the icon for this effect
    /// </summary>
    public virtual IconUsage Icon { get; protected set; } = FontAwesome.Solid.Circle;

    /// <summary>
    /// Who this Effect is effecting
    /// </summary>
    protected Entity Effector { get; private set; }

    /// <summary>
    /// If it's something that's based on a condition
    /// </summary>
    public abstract bool IsInfinite { get; set; }

    /// <summary>
    /// when the effect started
    /// </summary>
    public double? StartTime = null;

    /// <summary>
    /// is it effecting?
    /// </summary>
    public bool Active = false;

    /// <summary>
    /// The time between effect
    /// </summary>
    public virtual double Interval { get; protected set; } = new Second(1);

    /// <summary>
    /// How long the effect lasts
    /// </summary>
    public virtual int Duration { get; protected set; }

    /// <summary>
    /// This is how we tell where we are in the effect lifetime
    /// </summary>
    public double Time { get; private set; }

    /// <summary>
    /// How much of this effect is applied
    /// </summary>
    public int Stack;

    /// <summary>
    /// The current step
    /// </summary>
    public int CurrentStep = 1;

    /// <summary>
    /// Defines how the effect is reset in case you're using the effect as a reference,
    /// and you can customize it for children classes
    /// </summary>
    public virtual void Reset()
    {
        CurrentStep = 1;
        Time = 0;
        StartTime = null;
        Stack = 1;
        Active = false;
    }

    /// <summary>
    /// Restarts duration tracking without changing stacks.
    /// </summary>
    public virtual void RestartLifetime(double time)
    {
        StartTime = time;
        Time = time;
        CurrentStep = 1;
    }

    /// <summary>
    /// Clears lifetime anchor so it is re-based on next update tick.
    /// </summary>
    public virtual void RestartLifetime()
    {
        StartTime = null;
        Time = 0;
        CurrentStep = 1;
    }

    /// <summary>
    /// Set the effector
    /// </summary>
    /// <param name="entity">the entity</param>
    public void SetEffector(Entity entity) => Effector = entity;

    /// <summary>
    /// Set the time!
    /// </summary>
    /// <param name="time">time</param>
    public void SetTime(double time) => Time = time;

    /// <summary>
    /// The elapsed time
    /// </summary>
    /// <returns>elapsed time</returns>
    public double? ElapsedTime() => Time - StartTime;

    /// <summary>
    /// How this Effect will affect!
    /// </summary>
    public abstract void Handle();

    public Action OnRemove;
}
