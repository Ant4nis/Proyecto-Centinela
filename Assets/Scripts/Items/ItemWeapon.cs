using UnityEngine;

/// <summary>Enumeración que define si un arma es de una mano o de dos manos.</summary>
public enum HandedWeapon
{
    OneHanded,
    TwoHanded
}

/// <summary>Enumeración que define el tipo de arma: cuerpo a cuerpo o a distancia.</summary>
public enum WeaponType
{
    Melee,
    Distance
}

/// <summary>Enumeración que define la calidad del arma.</summary>
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
    /// Representa un arma del jugador, heredando de ItemData.
    /// 
    /// Funcionalidades:
    /// 1. Define el tipo, calidad y configuración general del arma.
    /// 2. Permite ajustar daño, velocidad de ataque, precisión y munición de cada arma.
    /// </summary>
    [CreateAssetMenu(menuName = "Items/Weapon")]
    public class ItemWeapon : ItemData
    {
        [Header("Manos ocupadas, tipo y calidad")]
        [Tooltip("Determina si el arma es de una mano o dos manos.")]
        public HandedWeapon HandedWeapon;
        [Tooltip("Define si el arma es de cuerpo a cuerpo o de distancia.")]
        public WeaponType Type;
        [Tooltip("Calidad del arma (Common, Rare, Epic, Legendary).")]
        public WeaponQuality Quality;

        [Header("Configuración")]
        [Tooltip("Daño base que inflige el arma.")]
        public float Damage;
        [Tooltip("Tiempo mínimo entre ataques consecutivos.")]
        public float TimeBetweenAttacks;
        [Tooltip("Precisión mínima del arma (mayor valor = menor precisión).")]
        public float MinAccuracy;
        [Tooltip("Precisión máxima del arma (menor valor = mayor precisión).")]
        public float MaxAccuracy;
        [Tooltip("Cantidad de munición consumida por disparo (solo para armas de distancia).")]
        public float Ammo;
    }
}