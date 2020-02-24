using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [Header("Stats")]
    public int RoomWidthMax;
    public int RoomWidthMin;

    public int RoomHeightMax;
    public int RoomHeightMin;

    public int RoomCountMax;
    public int RoomCountMin;

    public int HallLengthMin;
    public int HallLengthMax;

    public int HatchLengthMin;
    public int HatchLengthMax;

    public int BoundsHorizontal;
    public int BoundsVertical;

    //Settings
    public enum GridLocation { BottomLeft, BottomCenter, BottomRight, CenterLeft, Center, CenterRight, TopLeft, TopCenter, TopRight};

    [Header("Settings")]
    public GridLocation StartLocation;
    public GridLocation EndLocation;

    [Range(1 , 4)]
    public int RoomConnectionsMax;

    public float GridSize = 0.5f;

    [Header("References")]
    public GameObject GroundTile;
    public GameObject BackGroundTile;
    public Transform DungeonHolder;

    //Variables
    public GameObject[,] GridSpace;
    private int roomCount;

    //Builds Phase
    private void Awake() {
        GridSpace = new GameObject[BoundsHorizontal, BoundsVertical];
        BuildLevel();
    }

    private void BuildLevel() {

        roomCount = Random.Range(RoomCountMin, RoomCountMax);

        //Runs for each room
        //Creates Room
        int width = Random.Range(RoomWidthMin + 2, RoomWidthMax + 2);
        int height = Random.Range(RoomHeightMin + 2, RoomHeightMax + 2);
        Vector2 startPos = GetPosition(StartLocation);
        SpawnRoom(startPos, new Vector2(width, height), new Vector2(1, 1));
    }

    //Creates the room
    private void SpawnRoom(Vector2 start, Vector2 dimensions, Vector2 direction) {
        //Creates Room in the Hierarchy 
        GameObject roomParent = new GameObject();
        roomParent.transform.parent = DungeonHolder;

        for(int y = 0; y < dimensions.y; y++) {
            for(int x = 0; x < dimensions.x; x++) {
                //If its at an edge
                if(y == 0 || y == dimensions.y - 1 || x == 0 || x == dimensions.x - 1) {
                    float posX = start.x + (x * direction.x);
                    float posY = start.y + (y * direction.y);

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        continue;

                    GameObject block = Instantiate(GroundTile, roomParent.transform);
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                }
            }
        }

        roomCount--;

        //Selects if there is adjacent rooms
        List<int> directions = new List<int> {1, 2, 3, 4};
        for (int i = 0; i < Random.Range(1, RoomConnectionsMax + 1); i++)
        {

            if (roomCount == 0)
                break;

            int tempRoom = Random.Range(0, directions.Count);
            switch (directions[tempRoom]) {
                //Left
                case 1: HallWay(new Vector2(start.x,start.y + 1), -1, roomParent.transform); break;
                //Top
                case 2:
                    break;
                //Right
                case 3: HallWay(new Vector2(start.x + (dimensions.x * direction.x), start.y + 1), 1, roomParent.transform); break;
                //Bottom
                case 4:
                    break;

            }
            directions.Remove(directions[tempRoom]);
        }
    }

    public void HallWay(Vector2 start, int pos, Transform parent) {

        Vector2 dimensions = new Vector2(Random.Range(HallLengthMin, HallLengthMax),4);

        for(int y = 0; y < dimensions.y; y++) {
            for(int x = 0; x < dimensions.x; x++) {
                //If its at an edge
                if(x == 0 || x == dimensions.x - 1) {
                    float posX = start.x + (x * pos);
                    float posY = start.y + y;

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        continue;

                    GameObject block = Instantiate(GroundTile, parent);
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                }
            }
        }
    }

    public void Hatch(Vector2 start, int pos, Transform parent) {

        Vector2 dimensions = new Vector2(4, Random.Range(HatchLengthMin, HatchLengthMax));

        for(int y = 0; y < dimensions.y; y++) {
            for(int x = 0; x < dimensions.x; x++) {
                //If its at an edge
                if(y == 0 || y == dimensions.y - 1) {
                    float posX = start.x + x;
                    float posY = start.y + (y * pos);

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        continue;

                    GameObject block = Instantiate(GroundTile, parent);
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                }
            }
        }

    }

    private Vector2 GetPosition(GridLocation gridLocation) {

        Vector2 finalPosition = new Vector2();

        switch(gridLocation) {
            //Bottom
            case GridLocation.BottomLeft:   finalPosition = new Vector2(0, 0);    break;
            case GridLocation.BottomCenter: finalPosition = new Vector2(Mathf.FloorToInt(BoundsHorizontal / 2), 0); break;
            case GridLocation.BottomRight:  finalPosition = new Vector2(BoundsHorizontal, 0);   break;
            //Center
            case GridLocation.CenterLeft: finalPosition = new Vector2(0, Mathf.FloorToInt(BoundsVertical / 2)); break;
            case GridLocation.Center: finalPosition = new Vector2(Mathf.FloorToInt(BoundsHorizontal / 2), Mathf.FloorToInt(BoundsVertical / 2)); break;
            case GridLocation.CenterRight: finalPosition = new Vector2(BoundsHorizontal, Mathf.FloorToInt(BoundsVertical / 2)); break;
            //Top
            case GridLocation.TopLeft: finalPosition = new Vector2(0, BoundsVertical); break;
            case GridLocation.TopCenter: finalPosition = new Vector2(Mathf.FloorToInt(BoundsHorizontal / 2), BoundsVertical); break;
            case GridLocation.TopRight: finalPosition = new Vector2(BoundsHorizontal, BoundsVertical); break;
        }

        return finalPosition;
    }

    //Check Phase
}
