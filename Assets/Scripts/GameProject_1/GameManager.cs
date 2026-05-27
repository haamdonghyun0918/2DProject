using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { PlayerTurn, EnemyTurn, GameOver }
    public GameState currentState;

    private GameCharacter playerCharacter;
    private List<GameMonster> activeMonsters = new List<GameMonster>();
    private CardData selectedCard;

    private GameStage currentStage;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartBattle(GameCharacter character, List<GameMonster> monsters, GameStage stage)
    {
        playerCharacter = character;
        currentStage = stage;
        activeMonsters.Clear();
        activeMonsters.AddRange(monsters);

        currentState = GameState.PlayerTurn;
        Debug.Log("플레이어의 턴입니다!");
    }
    public bool UseCardOnMonster(CardData card, GameMonster targetMonster)
    {
        if (currentState != GameState.PlayerTurn) return false;

        targetMonster.TakeDamage(card.Damage);
        Debug.Log($"{card.Name} 카드로 공격!! (데미지: {card.Damage})");

        if (targetMonster.IsDead())
        {
            activeMonsters.Remove(targetMonster);
            Destroy(targetMonster.gameObject);
        }

        if (currentStage != null)
        {
            currentStage.RefillUsedCard();
        }

        if (activeMonsters.Count == 0)
        {
            GameOver(true);
            return true;
        }
        StartCoroutine(EnemyTurnRoutine());
        return true;
    }

    private IEnumerator EnemyTurnRoutine()
    {
        currentState = GameState.EnemyTurn;
        Debug.Log("상대방 턴");

        foreach (var monster in activeMonsters)
        {
            if (monster == null) continue;

            yield return new WaitForSeconds(0.5f);

            int damage = monster.GetAttackPower();
            playerCharacter.TakeDamage(damage);

            if (playerCharacter.IsDead())
            {
                GameOver(false);
                yield break;
            }
        }
        Debug.Log("상대 턴 종료! 플레이어 턴!!");
        currentState = GameState.PlayerTurn;
    }
    private void GameOver(bool isWin)
    {
        currentState = GameState.GameOver;
        if (StageManager.Instance != null && playerCharacter != null)
        {
            StageManager.Instance.SaveBattleResult(isWin, playerCharacter.GetCurrentHp());
        }

        UiManager.Instance.OpenGameMainScene();
        UiManager.Instance.CloseStageUi();
    }
}