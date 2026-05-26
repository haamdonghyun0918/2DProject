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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartBattle(GameCharacter character, List<GameMonster> monsters)
    {
        playerCharacter = character;
        activeMonsters = monsters;
        currentState = GameState.PlayerTurn;
        Debug.Log("플레이어의 턴입니다!");
    }
    public void SelectCard(CardData card)
    {
        if (currentState != GameState.PlayerTurn) return;
        selectedCard = card;
        Debug.Log($"{card.Name}카드를 선택! (데미지: {card.Damage}, 공격할 몬스터를 선택하세요!");
    }
    public void AttackMonster(GameMonster targetMonster)
    {
        if (currentState != GameState.PlayerTurn || selectedCard == null) return;

        targetMonster.TakeDamage(selectedCard.Damage);

        if (targetMonster.IsDead())
        {
            activeMonsters.Remove(targetMonster);
            Destroy(targetMonster.gameObject);
        }
        selectedCard = null;
        FindAnyObjectByType<GameStage>().RefillUsedCard();

        if (activeMonsters.Count == 0)
        {
            GameOver(true);
            return; ;
        }
        StartCoroutine(EnemyTurnRoutine());
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
                GameOver(true);
                yield break;
            }
        }
        Debug.Log("상대 턴 종료! 플레이어 턴!!");
        currentState = GameState.PlayerTurn;
    }
    private void GameOver(bool isWin)
    {
        currentState = GameState.GameOver;
        if (isWin) Debug.Log("모든 적을 처리하였습니다! 메인화면으로 돌아갑니다!");
        else Debug.Log("플레이어가 쓰러졌습니다... 메인 화면으로 돌아갑니다");
    }
}