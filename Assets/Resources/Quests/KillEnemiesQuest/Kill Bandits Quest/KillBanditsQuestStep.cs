using UnityEngine;

public class KillBanditsQuestStep : QuestStep
{
    private string firstEnemy = "Bandit Archer";
    private string secondEnemy = "Bandit Fighter";
    private int firstEnemyKillsNeeded = 5;
    private int secondEnemyKillsNeeded = 5;
    private int firstEnemyKills = 0;
    private int secondEnemyKills = 0;

    private void Start()
    {
        stepDescription = "Kill 5 Bandit Archers and 5 Bandit Fighters";
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
