﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using Unity.VisualScripting;

public class PathfindingDebugStepVisual : MonoBehaviour {

    public static PathfindingDebugStepVisual Instance { get; private set; }

    [SerializeField] private Transform pfPathfindingDebugStepVisualNode;
    private List<Transform> visualNodeList;
    private List<GridSnapshotAction> gridSnapshotActionList;
    private bool autoShowSnapshots;
    private float autoShowSnapshotsTimer;
    private Transform[,] visualNodeArray;
    private float size;

    private void Awake() {
        Instance = this;
        visualNodeList = new List<Transform>();
        gridSnapshotActionList = new List<GridSnapshotAction>();
    }

    public void Setup(Grid<PathNode> grid) {
        visualNodeArray = new Transform[grid.width, grid.height];
        size = grid.cellSize;
        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                Vector3 gridPosition = new(grid.GetLocalPosition(x, y).x + grid.cellSize * 0.5f, grid.GetLocalPosition(x, y).y + grid.cellSize * 0.5f);
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x, y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }
        HideNodeVisuals();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ShowNextSnapshot();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            autoShowSnapshots = true;
        }

        if (autoShowSnapshots) {
            float autoShowSnapshotsTimerMax = .05f;
            autoShowSnapshotsTimer -= Time.deltaTime;
            if (autoShowSnapshotsTimer <= 0f) {
                autoShowSnapshotsTimer += autoShowSnapshotsTimerMax;
                ShowNextSnapshot();
                if (gridSnapshotActionList.Count == 0) {
                    autoShowSnapshots = false;
                }
            }
        }
    }

    private void ShowNextSnapshot() {
        if (gridSnapshotActionList.Count > 0) {
            GridSnapshotAction gridSnapshotAction = gridSnapshotActionList[0];
            gridSnapshotActionList.RemoveAt(0);
            gridSnapshotAction.TriggerAction();
        }
    }

    public void ClearSnapshots() {
        gridSnapshotActionList.Clear();
    }

    public void TakeSnapshot(Grid<PathNode> grid, PathNode current, List<PathNode> openList, List<PathNode> closedList) {
        GridSnapshotAction gridSnapshotAction = new GridSnapshotAction();
        gridSnapshotAction.AddAction(HideNodeVisuals);
        
        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                PathNode pathNode = grid.GetGridObject(x, y);

                int gCost = pathNode.gCost;
                int hCost = pathNode.hCost;
                int fCost = pathNode.fCost;
                bool isCurrent = pathNode == current;
                bool isInOpenList = openList.Contains(pathNode);
                bool isInClosedList = closedList.Contains(pathNode);
                int tmpX = x;
                int tmpY = y;

                gridSnapshotAction.AddAction(() => {
                    Transform visualNode = visualNodeArray[tmpX, tmpY];
                    SetupVisualNode(visualNode, gCost, hCost, fCost);

                    Color backgroundColor = UtilsClass.GetColorFromString("636363");

                    if (isInClosedList) {
                        backgroundColor = new Color(1, 0, 0);
                    }
                    if (isInOpenList) {
                        backgroundColor = UtilsClass.GetColorFromString("009AFF");
                    }
                    if (isCurrent) {
                        backgroundColor = new Color(0, 1, 0);
                    }

                    visualNode.Find("sprite").GetComponent<SpriteRenderer>().color = backgroundColor;
                });
            }
        }

        gridSnapshotActionList.Add(gridSnapshotAction);
    }

    public void TakeSnapshotFinalPath(Grid<PathNode> grid, List<PathNode> path) {
        GridSnapshotAction gridSnapshotAction = new GridSnapshotAction();
        gridSnapshotAction.AddAction(HideNodeVisuals);
        
        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                PathNode pathNode = grid.GetGridObject(x, y);

                int gCost = pathNode.gCost;
                int hCost = pathNode.hCost;
                int fCost = pathNode.fCost;
                bool isInPath = path.Contains(pathNode);
                int tmpX = x;
                int tmpY = y;

                gridSnapshotAction.AddAction(() => { 
                    Transform visualNode = visualNodeArray[tmpX, tmpY];
                    SetupVisualNode(visualNode, gCost, hCost, fCost);

                    Color backgroundColor;

                    if (isInPath) {
                        backgroundColor = new Color(0, 1, 0);
                    } else {
                        backgroundColor = UtilsClass.GetColorFromString("636363");
                    }

                    visualNode.Find("sprite").GetComponent<SpriteRenderer>().color = backgroundColor;
                });
            }
        }

        gridSnapshotActionList.Add(gridSnapshotAction);
    }

    private void HideNodeVisuals() {
        foreach (Transform visualNodeTransform in visualNodeList) {
            SetupVisualNode(visualNodeTransform, 9999, 9999, 9999);
        }
    }

    private Transform CreateVisualNode(Vector3 position) {
        Transform visualNodeTransform = Instantiate(pfPathfindingDebugStepVisualNode, position, Quaternion.identity);
        return visualNodeTransform;
    }

    private void SetupVisualNode(Transform visualNodeTransform, int gCost, int hCost, int fCost) {
        float nomalize = size / visualNodeTransform.Find("sprite").localScale.x;
        visualNodeTransform.localScale = new Vector3 (nomalize, nomalize, 1f);
        if (fCost < 1000) {
            visualNodeTransform.Find("gCostText").GetComponent<TextMeshPro>().SetText(gCost.ToString());
            visualNodeTransform.Find("hCostText").GetComponent<TextMeshPro>().SetText(hCost.ToString());
            visualNodeTransform.Find("fCostText").GetComponent<TextMeshPro>().SetText(fCost.ToString());
        } else {
            visualNodeTransform.Find("gCostText").GetComponent<TextMeshPro>().SetText("0");
            visualNodeTransform.Find("hCostText").GetComponent<TextMeshPro>().SetText("0");
            visualNodeTransform.Find("fCostText").GetComponent<TextMeshPro>().SetText("0");
        }
    }

    private class GridSnapshotAction {

        private Action action;

        public GridSnapshotAction() {
            action = () => { };
        }

        public void AddAction(Action action) {
            this.action += action;
        }

        public void TriggerAction() {
            action();
        }

    }

}

