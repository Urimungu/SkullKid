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


    //Builds Phase
    private void Awake() {
        GridSpace = new GameObject[BoundsHorizontal, BoundsVertical];
        BuildLevel();
    }

    private void BuildLevel() {

        int roomCount = Random.Range(RoomCountMin, RoomCountMax);

        //Runs for each room
        for(int i = 0; i < roomCount; i++) {
            //Creates Room
            int width = Random.Range(RoomWidthMin + 2, RoomWidthMax + 2);
            int height = Random.Range(RoomHeightMin + 2, RoomHeightMax + 2);
            Vector2 startPos = GetPosition(StartLocation);
            SpawnRoom(startPos, new Vector2(width, height), new Vector2(1, 1));
        }
    }

    //Creates the room
    private void SpawnRoom(Vector2 start, Vector2 dimensions, Vector2 direction) {
        //Creates Room in the Heirarchy 
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
