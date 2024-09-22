using System.Collections.Generic;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Enemies;
using GentrysQuest.Game.Content.Families;
using GentrysQuest.Game.Content.Families.BraydenMesserschmidt;
using GentrysQuest.Game.Content.Families.JVee;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Location;

namespace GentrysQuest.Game.Content;

public class ContentManager
{
    public readonly List<IMap> Maps = new();
    public readonly List<Family> Families = new();
    public readonly List<Enemy> Enemies = new();
    public readonly List<Character> Characters = new();
    public readonly List<Weapon> Weapons = new();

    public ContentManager()
    {
        #region Maps

        Maps.Add(new TestMap());

        #endregion

        #region Families

        Families.Add(new TestFamily());
        Families.Add(new BraydenMesserschmidtFamily());
        Families.Add(new JVeeFamily());

        #endregion

        #region Enemies

        Enemies.Add(new TestEnemy());
        Enemies.Add(new AngryPedestrian());
        Enemies.Add(new AngryChineseMan());

        #endregion

        #region Characters

        Characters.Add(new BraydenMesserschmidt());

        #endregion

        #region Weapons

        Weapons.Add(new Knife());
        Weapons.Add(new Sword());
        Weapons.Add(new Bow());
        Weapons.Add(new Spear());
        Weapons.Add(new Hammer());
        Weapons.Add(new BraydensOsuPen());
        Weapons.Add(new BrodysBroadsword());

        #endregion
    }

    public IMap GetMap(string mapName)
    {
        foreach (IMap map in Maps)
        {
            if (map.Name == mapName) return map;
        }

        return null;
    }

    public Family GetFamily(string familyName)
    {
        foreach (Family family in Families)
        {
            if (family.Name == familyName) return family;
        }

        return null;
    }

    public Enemy GetEnemy(string enemyName)
    {
        foreach (Enemy enemy in Enemies)
        {
            if (enemy.Name == enemyName) return enemy;
        }

        return null;
    }

    public Character GetCharacter(string characterName)
    {
        foreach (Character character in Characters)
        {
            if (character.Name == characterName) return character;
        }

        return null;
    }

    public Weapon GetWeapon(string enemyName)
    {
        foreach (Weapon weapon in Weapons)
        {
            if (weapon.Name == enemyName) return weapon;
        }

        return null;
    }
}
