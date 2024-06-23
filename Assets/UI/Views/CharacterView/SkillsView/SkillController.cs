using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[Serializable]
public class SkillDetails
{
    public string Name;
    public string GUID;
    public Sprite Icon;
    public bool CanDrop;
}

public enum SkillPoolChangeType
{
    Pickup,
    Drop
}
public delegate void OnSkillPoolChangedDelegate(string[] SkillGuid, SkillPoolChangeType change);

/// <summary>
/// Generates and controls access to the Skill Database and SkillPool Data
/// </summary>
public class SkillController : MonoBehaviour
{
    [SerializeField]
    public List<Sprite> IconSprites;
    private static Dictionary<string, SkillDetails> m_SkillDatabase = new Dictionary<string, SkillDetails>();
    private List<SkillDetails> m_PlayerSkillPool = new List<SkillDetails>();
    public static event OnSkillPoolChangedDelegate OnSkillPoolChanged = delegate { };


    private void Awake()
    {
        PopulateDatabase();
    }

    private void Start()
    {
        //Add the SkillDatabase to the players SkillPool and let the UI know that some Skills have been picked up
        m_PlayerSkillPool.AddRange(m_SkillDatabase.Values);
        OnSkillPoolChanged.Invoke(m_PlayerSkillPool.Select(x => x.GUID).ToArray(), SkillPoolChangeType.Pickup);
    }

    /// <summary>
    /// Populate the database
    /// </summary>
    public void PopulateDatabase()
    {
        m_SkillDatabase.Add("8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA", new SkillDetails()
        {
            Name = "History of the Syndicate: 1501 to 1825 ",
            GUID = "8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("bg")),
            CanDrop = false
        });
        m_SkillDatabase.Add("992D3386-B743-4CD3-9BB7-0234A057C265", new SkillDetails()
        {
            Name = "Health Potion",
            GUID = "992D3386-B743-4CD3-9BB7-0234A057C265",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true
        });


    }

    /// <summary>
    /// Retrieve Skill details based on the GUID
    /// </summary>
    /// <param name="guid">ID to look up</param>
    /// <returns>Skill details</returns>
    public static SkillDetails GetSkillByGuid(string guid)
    {
        if (m_SkillDatabase.ContainsKey(guid))
        {
            return m_SkillDatabase[guid];
        }

        return null;
    }

}