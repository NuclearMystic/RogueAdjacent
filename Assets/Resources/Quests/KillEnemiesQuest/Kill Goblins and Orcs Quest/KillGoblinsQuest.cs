using UnityEngine;

public class KillGoblinsQuest : QuestStep
{
    private string firstEnemy = "Goblin";
    private string secondEnemy = "Orc";
    private int firstEnemyKillsNeeded = 5;
    private int secondEnemyKillsNeeded = 5;
    private int firstEnemyKills = 0;
    private int secondEnemyKills = 0;

    private void Start()
    {
        stepDescription = "Kill 5 Goblins and 5 Orcs";
        stepName = "Kill Bandits";
    }

    private void OnEnable()
    {
        GameEventsManager.instance.enemyEvents.onEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.enemyEvents.onEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(string enemyName)
    {
        bool updated = false;

        if (enemyName == firstEnemy)
        {
            firstEnemyKills++;
            updated = true;
        }
        else if (enemyName == secondEnemy)
        {
            secondEnemyKills++;
            updated = true;
        }
        if (updated)
        {
            UpdateStepDescription();
            if (firstEnemyKills >= firstEnemyKillsNeeded && secondEnemyKills >= secondEnemyKillsNeeded)
            {
                FinishQuestStep();
            }
        }
    }

    private void UpdateStepDescription()
    {
        stepDescription = $"Kill {firstEnemyKills}/{firstEnemyKillsNeeded} {firstEnemy}s and " +
                          $"{secondEnemyKills}/{secondEnemyKillsNeeded} {secondEnemy}s";
        GameEventsManager.instance.questEvents.QuestStepProgressChanged(questId);
    }

}
