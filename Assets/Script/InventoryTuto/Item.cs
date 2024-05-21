using UnityEngine;

public enum EquipType
{
    Helmet,
    Armor,
    Pants,
    Gloves,
    Belt,
    Boots,
    Weapon,
    Cloaks,
    Necklace,
    ring,
    None
}

[CreateAssetMenu(fileName="Item",menuName ="New Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Using,
        Ingredient,
        ETC
    }
    [SerializeField] private int _firstQuantity;
    
    [Header("Info")]
    public string itemName;
    public string Description;
    public Sprite itemImage;
    public ItemType itemType;
    public GameObject dropPrefab;//����� �������̶� �������� �ٸ��� ���� �־�� ��.
    public int itemQuantity;
    public EquipType equipType;

    [Header("CanStack")]
    public bool canStack;
    public int maxStackAmount;

    
}
