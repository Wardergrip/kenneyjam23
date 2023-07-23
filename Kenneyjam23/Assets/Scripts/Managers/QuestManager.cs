using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Leave choices empty! Choices are set through code")]
    [SerializeField] private DialogBlock _interactDialog = null;
    public DialogBlock InteractDialog
    {
        get { return _interactDialog; }
    }
    [SerializeField] private DialogBlock _exitDialog = null;
    public DialogBlock ExitDialog
    {
        get { return _exitDialog; }
    }

    private List<Quest> _quests = null;

    public int CurrentQuestIdx { get; private set; } = 0;
    public bool IsQuestlineCompleted { get; private set; } = false;

    [SerializeField] private GameObject _completeQuestFailMsg = null;
    [SerializeField] private GameObject _collectRewardsFailMsg = null;
    [SerializeField] private Transform _failMessageSocket = null;

    public void Initialize(List<Quest> quests)
    {
        // Set quests
        _quests = quests;

        // Create dialog block for the quests
        for (int questIdx = 0; questIdx < _quests.Count; ++questIdx)
        {
            // Set fail messages stuff
            _quests[questIdx].CompleteQuestFailMsg = _completeQuestFailMsg;
            _quests[questIdx].CollectRewardsFailMsg = _collectRewardsFailMsg;
            _quests[questIdx].FailMessageSocket = _failMessageSocket;

            // Quest dialog
            DialogBlock questDialog = ScriptableObject.CreateInstance<DialogBlock>();
            questDialog.ChoiceText = $"Quest {questIdx + 1}";
            questDialog.NpcSpeech = $"I need {_quests[questIdx].AmountRequired}x {_quests[questIdx].ItemRequired.ItemVisual.Name}.";

            // Quest complete predicate
            CompleteQuest completePredicate = ScriptableObject.CreateInstance<CompleteQuest>();
            completePredicate.Quest = _quests[questIdx];

            // Quest completed dialog
            DialogBlock questCompleted = ScriptableObject.CreateInstance<DialogBlock>();
            questCompleted.ChoiceText = "Here you go.";
            questCompleted.NpcSpeech = $"Thank you! Here are {_quests[questIdx].AmountRewarded}x {_quests[questIdx].ItemRewarded.ItemVisual.Name}.";
            questCompleted.Predicate = completePredicate;
            questCompleted.OnActivate.AddListener(_quests[questIdx].CompleteQuest);
            questCompleted.OnActivate.AddListener(CompleteQuest);

            // Quest collect predicate
            CollectQuest collectPredicate = ScriptableObject.CreateInstance<CollectQuest>();
            collectPredicate.Quest = _quests[questIdx];

            // Quest collected dialog
            DialogBlock questCollected = ScriptableObject.CreateInstance<DialogBlock>();
            questCollected.ChoiceText = "Thanks!";
            questCollected.Predicate = collectPredicate;
            questCollected.OnActivate.AddListener(_quests[questIdx].TryCollectRewards);

            // Quest completed choices
            List<DialogBlock> completedChoices = new List<DialogBlock>() { questCollected, ExitDialog };
            questCompleted.SetChoices(completedChoices);

            // Quest choices
            List<DialogBlock> questChoices = new List<DialogBlock>() { questCompleted, ExitDialog };
            questDialog.SetChoices(questChoices);

            // Set Quest dialog
            _quests[questIdx].QuestDialog = questDialog;
        }

        // Set quest as interact dialog choises
        UpdateQuestDialog();
    }

    private void UpdateQuestDialog()
    {
        if (_interactDialog != null)
        {
            List<DialogBlock> choices = new List<DialogBlock>();
            for (int questIdx = 0; questIdx <= CurrentQuestIdx; ++questIdx)
            {
                choices.Add(_quests[questIdx].QuestDialog);
            }
            choices.Add(_exitDialog);

            _interactDialog.SetChoices(choices);
        }
    }

    private void CompleteQuest()
    {
        // Ignore if the questline was completed already
        if (IsQuestlineCompleted) return;

        // Unlock the next quest
        if (++CurrentQuestIdx == _quests.Count)
        {
            // If this was the last quest, mark questline as completed
            IsQuestlineCompleted = true;
        }

        // Unlock the next quest dialog
        UpdateQuestDialog();
    }
}
