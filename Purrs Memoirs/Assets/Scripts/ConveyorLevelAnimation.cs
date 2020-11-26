using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorLevelAnimation : MonoBehaviour {

    public int CycleNumber { get; set; }
    public int ConveyorType { get; set; }
    public Sprite[] conveyorFrames;
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 1;
    private List<int> cycleId;
    private List<int> cornerId;
    private List<int> vergeId;
    
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AddCornerAndVergeIds(int corner, int verge, int cycle) {
        if (cycleId == null) {
            cycleId = new List<int>();
            cornerId = new List<int>();
            vergeId = new List<int>();
        }
        cycleId.Add(cycle);
        cornerId.Add(corner);
        vergeId.Add(verge);
    }

    public void MoveRight(int cycleIndex) {
        if (ConveyorType != 3) {
            StartCoroutine(MovingRight());
        }
        else {
            if (vergeId[cycleId.IndexOf(cycleIndex)] == 1) {
                StartCoroutine(MovingRightAngle());
            }
            else if (vergeId[cycleId.IndexOf(cycleIndex)] == 2) {
                StartCoroutine(MovingDownAngle());
            }
        }
    }

    public void MoveLeft(int cycleIndex) {
        if (ConveyorType != 3) {
            StartCoroutine(MovingLeft());
        }
        else {
            if (vergeId[cycleId.IndexOf(cycleIndex)] == 3) {
                StartCoroutine(MovingLeftAngle());
            }
            else if (vergeId[cycleId.IndexOf(cycleIndex)] == 4) {
                StartCoroutine(MovingUpAngle());
            }
        }
    }

    public void MoveLeftAngel() {
        StartCoroutine(MovingLeftAngle());
    }

    public void StopMoving() {
        StopAllCoroutines();
    }

    public void NextFrame(int numOfFrames) {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        currentFrame += numOfFrames;
        if (currentFrame >= conveyorFrames.Length) {
            currentFrame = 0;
        }
        spriteRenderer.sprite = conveyorFrames[currentFrame];
    }

    public void PreviousFrame(int numOfFrames) {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        currentFrame -= numOfFrames;
        if (currentFrame <= -1) {
            currentFrame = conveyorFrames.Length - 1;
        }
        spriteRenderer.sprite = conveyorFrames[currentFrame];
    }

    private IEnumerator MovingRight() {
        int i = conveyorFrames.Length - 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (true) {
            spriteRenderer.sprite = conveyorFrames[i];
            i--;
            if (i == -1) {
                i = conveyorFrames.Length - 1;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MovingLeft() {
        int i = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (true) {
            spriteRenderer.sprite = conveyorFrames[i];
            i++;
            if (i == conveyorFrames.Length) {
                i = 0;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MovingRightAngle() {
        int i = conveyorFrames.Length - 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (true) {
            spriteRenderer.sprite = conveyorFrames[i];
            i--;
            if (i == 3) {
                i = conveyorFrames.Length - 1;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MovingLeftAngle() {
        int i = 4;
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (true) {
            spriteRenderer.sprite = conveyorFrames[i];
            i++;
            if (i == conveyorFrames.Length) {
                i = 4;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MovingUpAngle() {
        int i = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (true) {
            spriteRenderer.sprite = conveyorFrames[i];
            i++;
            if (i == 4) {
                i = 0;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MovingDownAngle() {
        int i = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (true) {
            spriteRenderer.sprite = conveyorFrames[i];
            i--;
            if (i == -1) {
                i = 3;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
