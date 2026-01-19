
//게임 전체에서 사용하는 enum 정의
namespace My3DGame
{
    /// <summary>
    /// 이펙트 타입 정의
    /// </summary>
    public enum EffectType
    {
        NONE = -1,
        NORMAL,

    }

    /// <summary>
    /// 사운드 타입 정의
    /// </summary>
    public enum SoundType
    {
        NONE = -1,
        MUSIC,
        SFX,
    }

    /// <summary>
    /// 캐릭터 속성 정의
    /// </summary>
    public enum CharacterAttribute
    {
        Agility,
        Intellect,
        Stamina,
        Strength,
        Health,
        Mana,
    }

    /// <summary>
    /// 아이템 종류 정의
    /// </summary>
    public enum ItemType
    {
        Helmet = 0,
        Chest = 1,
        Pants = 2,
        Boots = 3,
        Pauldrons = 4,
        Gloves = 5,
        LeftWeapon = 6,
        RightWeapon = 7,
        Food,
        Default,
    }

    /// <summary>
    /// 인벤토리 타입
    /// </summary>
    public enum InventoryType
    {
        Inventory,      //창고형
        Equipment,      //장착형
        Shop,           //상점용
    }
}