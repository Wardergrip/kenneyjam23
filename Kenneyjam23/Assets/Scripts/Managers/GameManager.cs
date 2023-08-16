using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Quest> _quests = new List<Quest>();
    [SerializeField, Min(1)] private int _questsPerNPC = 1;

    [SerializeField] private List<Item> _chestItems = new List<Item>();
    public List<Item> ChestItems { get { return _chestItems; } }

    private List<Item> _availableItems = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateNPCQuests();
    }

    void GenerateNPCQuests()
    {
        // Set initial available items
        _availableItems.AddRange(_chestItems);

        // Get all quest managers in the scene
        List<NPC> npcs = FindObjectsOfType<NPC>().ToList();

        // Initialize quest lists for each NPC
        List<List<Quest>> npcQuests = new List<List<Quest>>();
        for (int i = 0; i < npcs.Count; ++i)
        {
            npcQuests.Add(new List<Quest>());
        }

        // Get quests for all NPCs
        for (int questIdx = 0; questIdx < _questsPerNPC; ++questIdx)
        {
            for (int npcIdx = 0; npcIdx < npcs.Count; ++npcIdx)
            {
                // Get all quests with a matching NPC type
                List<Quest> possibleQuests = _quests.Where(q => q.Type.Equals(npcs[npcIdx].Type) || q.Type == null).ToList();

                // Get all quests that require items which are available
                possibleQuests = possibleQuests.Where(q => _availableItems.Contains(q.ItemRequired)).ToList();

                // Get all quests that aren't used by the current NPC already
                possibleQuests = possibleQuests.Where(q => !npcQuests[npcIdx].Contains(q)).ToList();

                // Take a random quest from the list of possible quests
                Quest selectedQuest = possibleQuests[Random.Range(0, possibleQuests.Count)];

                // Add it to the current NPCs questline
                npcQuests[npcIdx].Add(selectedQuest);

                // Add the rewarded item from the selected quest to the list of available items
                if (!_availableItems.Contains(selectedQuest.ItemRewarded))
                {
                    _availableItems.Add(selectedQuest.ItemRewarded);
                }
            }
        }

        // Initialize the quest managers
        for (int npcIdx = 0; npcIdx < npcs.Count; ++npcIdx)
        {
            npcs[npcIdx].InitializeQuests(npcQuests[npcIdx]);
        }
    }
}
