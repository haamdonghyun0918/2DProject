using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // GameManager 역시 게임당 하나만 존재해야 하므로 싱글톤 패턴을 사용
    public static GameManager Instance { get; private set; }

    // 게임 상태를 만들어서 턴제 게임을 가능하게 만듦
    public enum GameState { PlayerTurn, EnemyTurn, GameOver }
    public GameState currentState;
    // 게임 캐릭터를 담을 그릇을 만듦(애니메이션을 표현하기 위해서)
    private GameCharacter playerCharacter;
    // 몬스터 리스트에서 활성화되는 몬스터들을 담을 리스트를 가져옴
    private List<GameMonster> activeMonsters = new List<GameMonster>();
    // 사용하려는 카드를 받는 변수
    private CardData selectedCard;
    // 현재 게임 스테이지를 받는 변수
    private GameStage currentStage;
    // 현재 턴을 계산하는 변수 추가
    public int currentTurn { get; private set; } = 1;

    private bool isBossPhase = false;
    private string currentBossId = "";
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartBattle(GameCharacter character, List<GameMonster> monsters, GameStage stage, string bossId)
    {
        playerCharacter = character;
        currentStage = stage;
        activeMonsters.Clear();
        activeMonsters.AddRange(monsters);

        currentBossId = bossId;
        isBossPhase = false;
        
        // 전투 시작 시 턴을 1로 초기화
        currentTurn = 1;
        
        if (currentStage != null)
        {
            currentStage.UpdateTurnText(currentTurn);
        }
        // 게임이 시작하면 무조건 플레이어 턴으로 시작
        currentState = GameState.PlayerTurn;
        Debug.Log("플레이어의 턴입니다!");
    }

    public bool UseCardOnMonster(CardData card, GameMonster targetMonster)
    {
        if (currentState != GameState.PlayerTurn) return false; // 카드를 내지 않으면 끝나지 플레이어의 턴이 끝나지 않음

        // 카드를 내면 상대편 턴으로 바꿉니다.
        currentState = GameState.EnemyTurn;

        StartCoroutine(ProcessPlayerAttackRoutine(card, targetMonster));
        return true;
    }

    private IEnumerator ProcessPlayerAttackRoutine(CardData card, GameMonster targetMonster)
    {
        // 💡 [캐릭터 공격] 애니메이션 길이를 계산하지 않고, 바로 데미지를 주고 0.5초만 짧게 대기합니다. (예전 방식 복구)
        playerCharacter.PlayAttackAnim();
        targetMonster.TakeDamage(card.Damage);
        Debug.Log($"{card.Name} 카드로 공격!! (데미지: {card.Damage})");

        yield return new WaitForSeconds(0.5f);

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

        if (targetMonster.IsDead())
        {
            if (isBossPhase)
            {
                // 💡 [보스 사망] 몬스터는 길이가 필요하므로 몬스터 스크립트의 GetCurrentAnimLength() 유지!
                targetMonster.PlayBossDieAnim();
                yield return null;
                yield return new WaitForSeconds(targetMonster.GetCurrentAnimLength());
            }
            else
            {
                yield return new WaitForSeconds(0.6f);
            }

            activeMonsters.Remove(targetMonster);
            Destroy(targetMonster.gameObject);
        }

        if (currentStage != null)
        {
            currentStage.RefillUsedCard();
        }

        if (activeMonsters.Count == 0)
        {
            if (!isBossPhase && !string.IsNullOrEmpty(currentBossId))
            {
                Debug.Log("잔몹 처치 완료! 보스 몬스터 등장!!");
                isBossPhase = true;

                GameMonster boss = currentStage.SpawnBossMonster(currentBossId);
                activeMonsters.Add(boss);

                yield return new WaitForSeconds(1.0f);

                Debug.Log("보스 등장! 플레이어에게 선공권이 주어집니다.");
                currentState = GameState.PlayerTurn;
                yield break;
            }
            else
            {
                GameOver(true);
                yield break;
            }
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

            // 💡 [몬스터 공격] 예전처럼 0.5초 고정 대기
            monster.PlayAttackAnim();
            yield return new WaitForSeconds(0.5f);

            // 💡 [캐릭터 피격] 캐릭터는 길이를 재지 않으므로 예전처럼 0.5초 고정 대기
            int damage = monster.GetAttackPower();
            playerCharacter.TakeDamage(damage);
            yield return new WaitForSeconds(0.5f);

            if (playerCharacter.IsDead())
            {
                yield return new WaitForSeconds(0.6f);
                GameOver(false);
                yield break;
            }

            if (monster.GetCurrentBleed() > 0)
            {
                monster.ApplyBleedDamage();
                yield return new WaitForSeconds(0.5f);

                if (monster.IsDead())
                {
                    if (isBossPhase)
                    {
                        // 💡 [보스 사망] 몬스터 스크립트의 길이를 그대로 씁니다.
                        monster.PlayBossDieAnim();
                        yield return null;
                        yield return new WaitForSeconds(monster.GetCurrentAnimLength());
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.6f);
                    }

                    activeMonsters.Remove(monster);
                    Destroy(monster.gameObject);

                    if (activeMonsters.Count == 0)
                    {
                        if (!isBossPhase && !string.IsNullOrEmpty(currentBossId))
                        {
                            Debug.Log("출혈로 잔몹 처치 완료! 보스 몬스터 등장!!");
                            isBossPhase = true;

                            GameMonster boss = currentStage.SpawnBossMonster(currentBossId);
                            activeMonsters.Add(boss);

                            yield return new WaitForSeconds(1.0f);

                            currentState = GameState.PlayerTurn;
                            yield break;
                        }
                        else
                        {
                            GameOver(true);
                            yield break;
                        }
                    }
                }
            }
        }

        Debug.Log("상대 턴 종료! 플레이어 턴!!");
        currentTurn++;
        if (currentStage != null)
        {
            currentStage.UpdateTurnText(currentTurn);
        }
        currentState = GameState.PlayerTurn;
    }

    private void GameOver(bool isWin)
    {
        currentState = GameState.GameOver;
        // 스테이지 승리 시, 현재 스테이지에서 사용한 턴 수를 누적 턴수에 저장하여 StageManager의 AddStageTurns에 연결
        if (isWin && StageManager.Instance != null)
        {
            StageManager.Instance.AddStageTurns(currentTurn);
        }

        if (StageManager.Instance != null && playerCharacter != null)
        {
            StageManager.Instance.SaveBattleResult(isWin, playerCharacter.GetCurrentHp());
        }

        if (isWin)
        {
            if (StageManager.Instance != null && StageManager.Instance.currentStageNum == 6)
            {
                Debug.Log("최종 스테이지를 클리어하였습니다! FinalClearUi가 열립니다");
                UiManager.Instance.OpenFinalClearUi();
            }
            else
            {
                Debug.Log("스테이지 클리어! ClearPopUp 오픈");
                UiManager.Instance.OpenClearPopUp();
            }
        }
        else
        {
            Debug.Log("스테이지 실패... FailPopUp 오픈");
            UiManager.Instance.OpenFailPopUp();
        }
    }
}