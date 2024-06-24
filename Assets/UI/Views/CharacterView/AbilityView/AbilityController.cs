using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AbilityDetails
{
    public string Name;
    public string GUID;
    public Sprite Icon;
    public bool CanDrop;
    public string SkillType;
    public string Level;
    public string Description;
    public int Effect;
}

public enum AbilityChangeType
{
    Pickup,
    Drop
}
public delegate void OnAbilityChangedDelegate(string[] itemGuid, AbilityChangeType change);

/// <summary>
/// Generates and controls access to the Ability Database and Player Abilities
/// </summary>
public class AbilityController : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> IconSprites;
    private static Dictionary<string, AbilityDetails> m_AbilityDatabase = new Dictionary<string, AbilityDetails>();
    private List<AbilityDetails> m_PlayerAbility = new List<AbilityDetails>(); // Fixed type from ItemDetails to AbilityDetails
    public static event OnAbilityChangedDelegate OnAbilitiesChanged = delegate { }; // Fixed delegate name to match the actual delegate type

    private void Awake()
    {
        PopulateDatabase();
    }

    private void Start()
    {
        // Add the AbilityDatabase to the player's abilities and let the UI know that some abilities have been picked up
        m_PlayerAbility.AddRange(m_AbilityDatabase.Values);
        OnAbilitiesChanged.Invoke(m_PlayerAbility.Select(x => x.GUID).ToArray(), AbilityChangeType.Pickup); // Fixed variable and enum names
    }

    /// <summary>
    /// Populate the database
    /// </summary>
    public void PopulateDatabase()
    {
        m_AbilityDatabase.Add("F6G7H8I9-J0K1-L2M3-N4O5-P6Q7R8S9T0U1", new AbilityDetails()
        {
            Name = "Perception + 20",
            GUID = "F6G7H8I9-J0K1-L2M3-N4O5-P6Q7R8S9T0U1",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("G7H8I9J0-K1L2-M3N4-O5P6-Q7R8S9T0U1V2", new AbilityDetails()
        {
            Name = "Charisma + 20",
            GUID = "G7H8I9J0-K1L2-M3N4-O5P6-Q7R8S9T0U1V2",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("syndicate")),
            CanDrop = true,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA", new AbilityDetails()
        {
            Name = "Mana + 20",
            GUID = "8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("syndicate")),
            CanDrop = false,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("992D3386-B743-4CD3-9BB7-0234A057C265", new AbilityDetails()
        {
            Name = "Health + 20",
            GUID = "992D3386-B743-4CD3-9BB7-0234A057C265",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("1B9C6CAA-754E-412D-91BF-37F22C9A0E7B", new AbilityDetails()
        {
            Name = "Damge + 20",
            GUID = "1B9C6CAA-754E-412D-91BF-37F22C9A0E7B",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("poison")),
            CanDrop = true,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });
        m_AbilityDatabase.Add("A1B2C3D4-E5F6-7G8H-9I0J-K1L2M3N4O5P6", new AbilityDetails()
        {
            Name = "Speed + 20",
            GUID = "A1B2C3D4-E5F6-7G8H-9I0J-K1L2M3N4O5P6",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("syndicate")),
            CanDrop = true,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("B2C3D4E5-F6G7-H8I9-J0K1-L2M3N4O5P6Q7", new AbilityDetails()
        {
            Name = "Stamina + 20",
            GUID = "B2C3D4E5-F6G7-H8I9-J0K1-L2M3N4O5P6Q7",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("C3D4E5F6-G7H8-I9J0-K1L2-M3N4O5P6Q7R8", new AbilityDetails()
        {
            Name = "Agility + 20",
            GUID = "C3D4E5F6-G7H8-I9J0-K1L2-M3N4O5P6Q7R8",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("poison")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("D4E5F6G7-H8I9-J0K1-L2M3-N4O5P6Q7R8S9", new AbilityDetails()
        {
            Name = "Intelligence + 20",
            GUID = "D4E5F6G7-H8I9-J0K1-L2M3-N4O5P6Q7R8S9",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("syndicate")),
            CanDrop = true,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("E5F6G7H8-I9J0-K1L2-M3N4-O5P6Q7R8S9T0", new AbilityDetails()
        {
            Name = "Luck + 20",
            GUID = "E5F6G7H8-I9J0-K1L2-M3N4-O5P6Q7R8S9T0",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Common",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });
        m_AbilityDatabase.Add("H8I9J0K1-L2M3-N4O5-P6Q7-R8S9T0U1V2W3", new AbilityDetails()
        {
            Name = "Endurance + 20",
            GUID = "H8I9J0K1-L2M3-N4O5-P6Q7-R8S9T0U1V2W3",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("I9J0K1L2-M3N4-O5P6-Q7R8-S9T0U1V2W3X4", new AbilityDetails()
        {
            Name = "Wisdom + 20",
            GUID = "I9J0K1L2-M3N4-O5P6-Q7R8-S9T0U1V2W3X4",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("J0K1L2M3-N4O5-P6Q7-R8S9-T0U1V2W3X4Y5", new AbilityDetails()
        {
            Name = "Stealth + 20",
            GUID = "J0K1L2M3-N4O5-P6Q7-R8S9-T0U1V2W3X4Y5",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("K1L2M3N4-O5P6-Q7R8-S9T0-U1V2W3X4Y5Z6", new AbilityDetails()
        {
            Name = "Dexterity + 20",
            GUID = "K1L2M3N4-O5P6-Q7R8-S9T0-U1V2W3X4Y5Z6",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("syndicate")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("L2M3N4O5-P6Q7-R8S9-T0U1-V2W3X4Y5Z6A7", new AbilityDetails()
        {
            Name = "Focus + 20",
            GUID = "L2M3N4O5-P6Q7-R8S9-T0U1-V2W3X4Y5Z6A7",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });

        m_AbilityDatabase.Add("M3N4O5P6-Q7R8-S9T0-U1V2-W3X4Y5Z6A7B8", new AbilityDetails()
        {
            Name = "Courage + 20",
            GUID = "M3N4O5P6-Q7R8-S9T0-U1V2-W3X4Y5Z6A7B8",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("syndicate")),
            CanDrop = true,
            SkillType = "Rare",
            Level = "1",
            Description = "Increases the chance of finding hidden items",
            Effect = 20,
        });
        // Removed duplicate "Bottle of Poison" entries with different GUIDs but same name and icon for brevity
    }

    /// <summary>
    /// Retrieve item details based on the GUID
    /// </summary>
    /// <param name="guid">ID to look up</param>
    /// <returns>Item details</returns>
    public static AbilityDetails GetItemByGuid(string guid)
    {
        if (m_AbilityDatabase.TryGetValue(guid, out AbilityDetails abilityDetails))
        {
            return abilityDetails;
        }

        return null;
    }
}