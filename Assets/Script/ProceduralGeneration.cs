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

    public int HallHeightMin;
    public int HallHeightMax;

    public int HatchLengthMin;
    public int HatchLengthMax;

    public int HatchWidthMin;
    public int HatchWidthMax;

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
    private int roomCountInitial;

    //Builds Phase
    private void Awake() {
        GridSpace = new GameObject[BoundsHorizontal, BoundsVertical];
        BuildLevel();
    }

    private void BuildLevel() {

        roomCount = Random.Range(RoomCountMin, RoomCountMax);
        roomCountInitial = roomCount;

        //Start Up
        int width = Random.Range(RoomWidthMin + 2, RoomWidthMax + 2);
        int height = Random.Range(RoomHeightMin + 2, RoomHeightMax + 2);
        Vector2 startPos = GetPosition(StartLocation);
        SpawnRoom(startPos, new Vector2(width, height), new Vector2(0, 0));

        //Places the Player
        //GameManager.Manager.SpawnPlayer((startPos + new Vector2(1,1)) * GridSize);
    }

    //Creates the room
    private void SpawnRoom(Vector2 start, Vector2 dimensions, Vector2 direction) {

        //Creates Room in the Hierarchy 
        GameObject roomParent = new GameObject();
        roomParent.name = "Room: " + roomCount;
        roomParent.transform.parent = DungeonHolder;

        for(int y = 0; y < dimensions.y; y++) {
            for(int x = 0; x < dimensions.x; x++) {
                //If its at an edge
                if(y == 0 || y == dimensions.y - 1 || x == 0 || x == dimensions.x - 1) {
                    float posX = start.x + (x * (direction.x == 0 ? 1 : direction.x));
                    float posY = start.y + (y * (direction.y == 0 ? 1 : direction.y));

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        continue;

                    GameObject block = Instantiate(GroundTile, roomParent.transform);
                    block.name = "Room Edge :( " + x + ", " + y + ")";
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                } else {
                    float posX = start.x + (x * (direction.x == 0 ? 1 : direction.x));
                    float posY = start.y + (y * (direction.y == 0 ? 1 : direction.y));

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        Destroy(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)]);

                    GameObject block = Instantiate(BackGroundTile, roomParent.transform);
                    block.name = "Room Middle :( " + x + ", " + y + ")";
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                }
            }
        }

        //Selects if there is adjacent rooms
        List<int> directions = new List<int> {1, 2, 3, 4};

        //Stops from overlapping directions
        if(direction == new Vector2(1, 0)) directions.Remove(directions[0]);
        else if(direction == new Vector2(-1,0)) directions.Remove(directions[2]);
        else if(direction == new Vector2(0, 1)) directions.Remove(directions[3]);
        else if(direction == new Vector2(0, -1)) directions.Remove(directions[1]);

        //Selects the new directions
        for (int i = 0; i < Random.Range(1, RoomConnectionsMax + ( roomCount == roomCountInitial? 1 : 0)); i++){
            if(roomCount <= 0) return;
            --roomCount;
            int tempRoom = Random.Range(0, directions.Count);
            switch (directions[tempRoom]) {
                //Left
                case 1:
                    float newY1 = direction.y >= 0 ? start.y : start.y - dimensions.y + 1;
                    HallWay(new Vector2(start.x + 1, newY1), -1, roomParent.transform); 
                break;
                //Top
                case 2:
                    float newX2 = direction.x >= 0 ? Random.Range(start.x + 1, start.x + dimensions.x - 2) : Random.Range(start.x - dimensions.x, start.x - 3);
                    float newY2 = direction.y >= 0 ? start.y + dimensions.y : start.y;

                    Hatch(new Vector2(Mathf.FloorToInt(newX2), newY2 - 1), 1, roomParent.transform);
                break;
                //Right
                case 3:
                    float newY3 = direction.y >= 0 ? start.y : start.y - dimensions.y + 1;
                    HallWay(new Vector2(start.x + dimensions.x - 1, newY3), 1, roomParent.transform); 
                break;
                //Bottom
                case 4:
                    float newX4 = direction.x >= 0 ? Random.Range(start.x, start.x + dimensions.x - 3) : Random.Range(start.x - dimensions.x, start.x - 3);
                    float newY4 = direction.y >= 0 ? start.y : start.y - dimensions.y;

                    Hatch(new Vector2(Mathf.FloorToInt(newX4), newY4 + 1), -1, roomParent.transform); 
                break;

            }
            directions.Remove(directions[tempRoom]);
        }
    }

    public void HallWay(Vector2 start, int pos, Transform parent) {
        Vector2 dimensions = new Vector2(Random.Range(HallLengthMin, HallLengthMax),Random.Range(HallHeightMin, HallHeightMax));

        for(int y = 0; y < dimensions.y; y++) {
            for(int x = 0; x < dimensions.x; x++) {
                //If its at an edge
                if(y == 0 || y == dimensions.y - 1) {
                    float posX = start.x + (x * pos);
                    float posY = start.y + y;

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        continue;

                    GameObject block = Instantiate(GroundTile, parent);
                    block.name = "Hall Edge :( " + x + ", " + y + ")";
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                } else {
                    float posX = start.x + (x * pos);
                    float posY = start.y + y;

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        Destroy(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)]);

                    GameObject block = Instantiate(BackGroundTile, parent);
                    block.name = "Hall Middle :( " + x + ", " + y + ")";
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                }
            }
        }

        int width = Random.Range(RoomWidthMin + 2, RoomWidthMax + 2);
        int height = Random.Range(RoomHeightMin + 2, RoomHeightMax + 2);
        Vector2 startPos = new Vector2(start.x + (dimensions.x * pos) - pos,start.y);
        SpawnRoom(startPos, new Vector2(width, height), new Vector2(pos, 0));
    }

    public void Hatch(Vector2 start, int pos, Transform parent) {
        Vector2 dimensions = new Vector2(Random.Range(HatchWidthMin, HatchWidthMax), Random.Range(HatchLengthMin, HatchLengthMax));

        for(int y = 0; y < dimensions.y; y++) {
            for(int x = 0; x < dimensions.x; x++) {
                //If its at an edge
                if(x == 0 || x == dimensions.x - 1) {
                    float posX = start.x + x;
                    float posY = start.y + (y * pos);

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        continue;

                    GameObject block = Instantiate(GroundTile, parent);
                    block.name = "Hatch Edge :( " + x + ", " + y + ")";
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                } else {
                    float posX = start.x + x;
                    float posY = start.y + (y * pos);

                    if(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] != null)
                        Destroy(GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)]);

                    GameObject block = Instantiate(BackGroundTile, parent);
                    block.name = "Hatch Middle :( " + x + ", " + y + ")";
                    block.transform.position = new Vector2(posX * GridSize, posY * GridSize);
                    GridSpace[Mathf.FloorToInt(posX), Mathf.FloorToInt(posY)] = block;
                }
            }
        }
        int width = Random.Range(RoomWidthMin + 2, RoomWidthMax + 2);
        int height = Random.Range(RoomHeightMin + 2, RoomHeightMax + 2);
        Vector2 startPos = new Vector2(start.x, start.y + (dimensions.y * pos) - pos);
        SpawnRoom(startPos, new Vector2(width, height), new Vector2(0, pos));
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
