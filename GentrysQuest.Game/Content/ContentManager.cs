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

public static class ContentManager
{
    public static readonly List<Map> MAPS = new();
    public static readonly List<Family> FAMILIES = new();
    public static readonly List<Enemy> ENEMIES = new();
    public static readonly List<Character> CHARACTERS = new();
    public static readonly List<Weapon> WEAPONS = new();

    public static void LoadContent()
    {
        #region Maps

        MAPS.Add(new TestMap());

        #endregion

        #region Families

        FAMILIES.Add(new TestFamily());
        FAMILIES.Add(new BraydenMesserschmidtFamily());
        FAMILIES.Add(new JVeeFamily());

        #endregion

        #region Enemies

        ENEMIES.Add(new TestEnemy());
        ENEMIES.Add(new AngryPedestrian());
        ENEMIES.Add(new AngryChineseMan());

        #endregion

        #region Characters

        CHARACTERS.Add(new BraydenMesserschmidt());

        #endregion

        #region Weapons

        WEAPONS.Add(new Knife());
        WEAPONS.Add(new Sword());
        WEAPONS.Add(new Bow());
        WEAPONS.Add(new Spear());
        WEAPONS.Add(new Hammer());
        WEAPONS.Add(new BraydensOsuPen());
        WEAPONS.Add(new BrodysBroadsword());

        #endregion
    }

    public static Map GetMap(string mapName)
    {
        foreach (Map map in MAPS)
        {
            if (map.Name == mapName) return map;
        }

        return null;
    }

    public static Family GetFamily(string familyName)
    {
        foreach (Family family in FAMILIES)
        {
            if (family.Name == familyName) return family;
        }

        return null;
    }

    public static Enemy GetEnemy(string enemyName)
    {
        foreach (Enemy enemy in ENEMIES)
        {
            if (enemy.Name == enemyName) return enemy;
        }

        return null;
    }

    public static Character GetCharacter(string characterName)
    {
        foreach (Character character in CHARACTERS)
        {
            if (character.Name == characterName) return character;
        }

        return null;
    }

    public static Weapon GetWeapon(string enemyName)
    {
        foreach (Weapon weapon in WEAPONS)
        {
            if (weapon.Name == enemyName) return weapon;
        }

        return null;
    }
}
