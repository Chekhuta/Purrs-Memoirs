using System.Collections;
using UnityEngine;

public class MenuConveyors : MonoBehaviour {


    public Animator[] leftSpawners;
    public Animator[] rightSpawners;
    public ConveyorLevelAnimation[] conveyors;
    public Transform[] buttonsPage;
    private float shiftDistanceX = 7.2f;
    private enum ShiftDirection { Left = -1, Right = 1 }

    public void ShiftPagesToLeft() {
        MoveSpawners(leftSpawners, "Move Back");
        MoveSpawners(rightSpawners, "Move");
        Coroutine conveyorsCoroutine = StartCoroutine(MoveConveyorsToLeft());
        StartCoroutine(ShiftPages(conveyorsCoroutine, (int)ShiftDirection.Left));
    }

    public void ShiftPagesToRight() {
        MoveSpawners(leftSpawners, "Move");
        MoveSpawners(rightSpawners, "Move Back");
        Coroutine conveyorsCoroutine = StartCoroutine(MoveConveyorsToRight());
        StartCoroutine(ShiftPages(conveyorsCoroutine, (int)ShiftDirection.Right));
    }

    private IEnumerator MoveConveyorsToLeft() {
        while (true) {
            foreach (ConveyorLevelAnimation c in conveyors) {
                c.NextFrame(2);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator MoveConveyorsToRight() {
        while (true) {
            foreach (ConveyorLevelAnimation c in conveyors) {
                c.PreviousFrame(2);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator ShiftPages(Coroutine conveyorsCoroutine, int direction) {
        Vector3[] startPagesPositions = new Vector3[buttonsPage.Length];
        for (int i = 0; i < startPagesPositions.Length; i++) {
            startPagesPositions[i] = buttonsPage[i].localPosition;
        }

        for (int repeatCount = 1; repeatCount < 31; repeatCount++) {
            Vector3 shift = new Vector3(0.12f * repeatCount * 2, 0, 0);
            for (int i = 0; i < startPagesPositions.Length; i++) {
                buttonsPage[i].localPosition = startPagesPositions[i] + (shift * direction);
            }
            if (repeatCount == 30) {
                StopCoroutine(conveyorsCoroutine);
            }
            yield return new WaitForSeconds(0.01f);
        }
        MoveSpawners(leftSpawners, "Stop");
        MoveSpawners(rightSpawners, "Stop");
        Sounds.GetInstance().StopConveyorSound();
    }

    private void MoveSpawners(Animator[] spawners, string direction) {
        for (int i = 0; i < spawners.Length; i++) {
            spawners[i].SetTrigger(direction);
        }
    }
}
