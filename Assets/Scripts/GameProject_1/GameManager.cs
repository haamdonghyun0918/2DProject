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

        // 카드를 냈을 때 몬스터가 죽는 연출 도중 또 카드를 내는 버그를 막기 위해 상태를 잠급니다.
        currentState = GameState.EnemyTurn;

        StartCoroutine(ProcessPlayerAttackRoutine(card, targetMonster));
        return true;
    }

    private IEnumerator ProcessPlayerAttackRoutine(CardData card, GameMonster targetMonster)
    {
        playerCharacter.PlayAttackAnim();

        targetMonster.TakeDamage(card.Damage);
        Debug.Log($"{card.Name} 카드로 공격!! (데미지: {card.Damage})");

        if (card.Heal > 0)
        {
            playerCharacter.HealHp(card.Heal);
            Debug.Log($"흡혈로 인하여 체력이 {card.Heal}만큼 회복되었습니다");
        }

        if (card.Bleed > 0)
        {
            targetMonster.AddBleed(card.Bleed);
            Debug.Log($"{targetMonster.name}에게 출혈을 주었습니다. 매 턴마다 {targetMonster.GetCurrentBleed()}의 데미지를 줍니다");
        }

        //1차 대기: 피격(Damaged) 애니메이션과 데미지 숫자가 뜨는 것을 볼 시간
        yield return new WaitForSeconds(0.5f);

        if (targetMonster.IsDead())
        {
            // 2차 대기(사망 여운): 몬스터가 죽었다면, 쓰러지는 애니메이션이 다 끝날 때까지 '추가로' 더 기다려줍니다.
            yield return new WaitForSeconds(0.6f);

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
            yield break; // 전투 종료 시 에러 방지를 위해 코루틴을 완벽히 탈출합니다.
        }

        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        currentState = GameState.EnemyTurn;
        Debug.Log("상대방 턴");

        foreach (var monster in activeMonsters.ToArray())
        {
            if (monster == null) continue;

            monster.PlayAttackAnim();
            yield return new WaitForSeconds(0.5f);

            int damage = monster.GetAttackPower();
            playerCharacter.TakeDamage(damage);

            // 플레이어가 맞고 움찔하는 시간을 줍니다.
            yield return new WaitForSeconds(0.5f);

            if (playerCharacter.IsDead())
            {
                // 플레이어가 죽었을 때도 바로 팝업이 뜨지 않고 쓰러지는 걸 볼 시간을 줍니다.
                yield return new WaitForSeconds(0.6f);
                GameOver(false);
                yield break;
            }

            if (monster.GetCurrentBleed() > 0)
            {
                monster.ApplyBleedDamage();

                // 출혈 데미지를 받고 움찔하는 시간을 줍니다.
                yield return new WaitForSeconds(0.5f);

                if (monster.IsDead())
                {
                    //출혈로 죽었을 때도 완전히 모션이 끝날 때까지 기다립니다.
                    yield return new WaitForSeconds(0.6f);

                    activeMonsters.Remove(monster);
                    Destroy(monster.gameObject);

                    if (activeMonsters.Count == 0)
                    {
                        GameOver(true);
                        yield break;
                    }
                }
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

        if (isWin)
        {
            Debug.Log("스테이지 클리어! ClearPopUp 오픈");
            UiManager.Instance.OpenClearPopUp();
        }
        else
        {
            Debug.Log("스테이지 실패... FailPopUp 오픈");
            UiManager.Instance.OpenFailPopUp();
        }
    }
}