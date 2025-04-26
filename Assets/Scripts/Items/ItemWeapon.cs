using UnityEngine;

public enum HandedWeapon
{
    OneHanded,
    TwoHanded
}

public enum WeaponType
{
    Melee,
    Distance
}

public enum WeaponQuality
{
    Common,
    Rare,
    Epic,
    Legendary
}
namespace Items
{
    /// <summary>
    /// Hereda toda la logica y aniade nuevas
    /// </summary>
    [CreateAssetMenu(menuName = "Items/Weapon")]
    public class ItemWeapon : ItemData
    {
        [Header("Manos ocupadas, tipo y calidad")]
        public HandedWeapon HandedWeapon;
        public WeaponType Type;
        public WeaponQuality Quality;
        
        [Header("Configuración")]
        public float Damage;
        public float TimeBetweenAttacks;
        public float MinAccuracy;
        public float MaxAccuracy;
        public float Ammo;


    }
}