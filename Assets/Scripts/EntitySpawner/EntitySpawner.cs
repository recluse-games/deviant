using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]
    EntityPrefab entityPrefab = default;

    private EncounterState _encounterStateComponentReference = default;
    public Tilemap battlefieldTilemapRef = default;
    public List<string> activeEntities = default;

    public void Start()
    {
        // Find Operations Are Expensive So the Less We Do The Better.
        battlefieldTilemapRef = GameObject.Find("/IsometricGrid/Battlefield").GetComponent<Tilemap>();
        _encounterStateComponentReference = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    async public void Update()
    {
        // Retrieve the Current Encounter From Shared State.
        Deviant.Encounter encounterState = _encounterStateComponentReference.GetEncounter();
        Deviant.Board board = encounterState.Board;
        List<string> currentEntities = new List<string>();
        Tilemap overlay = GameObject.Find("BattlefieldOverlay").GetComponent<Tilemap>();

        // Iterate Over 2d Tile Grid of Entities and Populate Battlefield
        for (int y = 0; y < board.Entities.Entities_.Count; y++)
        {
            for (int x = 0; x < board.Entities.Entities_[y].Entities.Count; x++)
            {
                string entityId = board.Entities.Entities_[y].Entities[x].Id;
                Deviant.Alignment entityAlignment = board.Entities.Entities_[y].Entities[x].Alignment;
                Vector3Int currentTileLocation = new Vector3Int(y, x, 0);

                currentEntities.Add(board.Entities.Entities_[y].Entities[x].Id);

                // Validate that we haven't already spawned this unit in and that it's not an empty placeholder object.
                if (this.activeEntities.Contains(entityId) == false && entityId != "" && entityId != null)
                {
                    EntityPrefab entity = Instantiate(entityPrefab);

                    // Set the Sprite Based off the entity class + alignment
                    entity.setSprite(board.Entities.Entities_[y].Entities[x].Class.ToString(), entityAlignment.ToString());
                    entity.transform.gameObject.name = "entity_" + entityId;

                    AlignSpriteToTile(entity, currentTileLocation);
                    Enable2DBoxCollider(entity);
                    FlipEnemyOrientation(entity, entityAlignment);
                    TagEntity(entity, entityAlignment);

                    // Set the entityid
                    entity.SetId(entityId);

                    // Add the new state observer to the entity
                    _encounterStateComponentReference.AddEntityObserver(GameObject.Find("entity_" + entityId));

                    // Update Active Entities
                    this.activeEntities.Add(board.Entities.Entities_[y].Entities[x].Id);
                }

                if (entityId != null)
                {
                    GameObject existingEntity = GameObject.Find("entity_" + entityId);

                    if (existingEntity != null)
                    {
                        Vector3Int existingEntityLocation = overlay.WorldToCell(existingEntity.transform.position);

                        if (existingEntityLocation.x != y || existingEntityLocation.y != x)
                        {
                            existingEntity.transform.position = overlay.CellToWorld(new Vector3Int(y, x, 0));
                            Entity existingEntityInstance = existingEntity.GetComponentInChildren<Entity>();
                            await existingEntityInstance.SetIdle();
                        }
                    }
                }
            }
        }

        foreach (var entityId in this.activeEntities)
        {
            if (currentEntities.Contains(entityId) == false)
            {
                Debug.Log("Removing: " + entityId);
                GameObject existingDeadEntity = GameObject.Find("entity_" + entityId);
                GameObject.Destroy(existingDeadEntity);
            }
        }
    }

    private void AlignSpriteToTile(EntityPrefab entity, Vector3Int currentCellPosition)
    {
        entity.transform.position = battlefieldTilemapRef.CellToWorld(currentCellPosition);
    }

    private void Enable2DBoxCollider(EntityPrefab entity)
    {
        entity.transform.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
    }

    private void TagEntity(EntityPrefab entity, Deviant.Alignment entityAlignment)
    {
        if (entityAlignment == Deviant.Alignment.Unfriendly)
        {
            entity.transform.gameObject.tag = "entity_unfriendly";
        }
        else
        {
            entity.transform.gameObject.tag = "entity_friendly";
        }
    }

    private void FlipEnemyOrientation(EntityPrefab entity, Deviant.Alignment entityAlignment)
    {
        if (entityAlignment == Deviant.Alignment.Unfriendly)
        {
            entity.setXFlip();
        }
    }
}