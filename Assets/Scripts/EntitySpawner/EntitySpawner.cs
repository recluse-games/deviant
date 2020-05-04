using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Google.Protobuf.WellKnownTypes;

public class EntitySpawner : MonoBehaviour
{
	[SerializeField]
	EntityPrefab entityPrefab = default;

	public EncounterState encounterStateRef = default;
	public Tilemap battlefieldTilemapRef = default;
	public List<string> activeEntities = default;

	public void Start() {
		// Find Operations Are Expensive So the Less We Do The Better.
		battlefieldTilemapRef = GameObject.Find("/IsometricGrid/Battlefield").GetComponent<Tilemap>();
		encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
	}

	public void Update() {
		// Retrieve the Current Encounter From Shared State.
		Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
		Deviant.Board board = encounterState.Board;

		// Iterate Over 2d Tile Grid of Entities and Populate Battlefield
		for (int y = 0; y < board.Entities.Entities_.Count; y++)
		{
			for (int x = 0; x < board.Entities.Entities_[y].Entities.Count; x++)
			{
				string entityId = board.Entities.Entities_[y].Entities[x].Id;
				Deviant.Alignment entityAlignment = board.Entities.Entities_[y].Entities[x].Alignment;
				Vector3Int currentTileLocation = new Vector3Int(x, y, 0);				

				// Validate that we haven't already spawned this unit in and that it's not an empty placeholder object.
				if (this.activeEntities.Contains(entityId) == false && entityId != "") {
					EntityPrefab entity = Instantiate(entityPrefab);
					
					// Set the Sprite Based off the entity class + alignment
					entity.setSprite(board.Entities.Entities_[y].Entities[x].Class.ToString(), entityAlignment.ToString());
					entity.transform.gameObject.name = "entity_" + entityId; 

					AlignSpriteToTile(entity, currentTileLocation);
					Enable2DBoxCollider(entity);
					EnableIdleAnimation(entity);
					FlipEnemyOrientation(entity, entityAlignment);

					// Update Active Entities
					this.activeEntities.Add(board.Entities.Entities_[y].Entities[x].Id);
				}
			}
		}
	}

	private void AlignSpriteToTile(EntityPrefab entity, Vector3Int currentCellPosition) {
		entity.transform.position = battlefieldTilemapRef.CellToWorld(currentCellPosition);
		entity.transform.position += new Vector3(0, .20f, 0);
	}

	private void Enable2DBoxCollider(EntityPrefab entity) {
		entity.transform.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
	}

	private void EnableIdleAnimation(EntityPrefab entity) {
		Animator entityAnimator = entity.transform.gameObject.GetComponentInChildren<Animator>();

		entity.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrrior-Idle");
		entityAnimator.enabled = true;
	}

	private void FlipEnemyOrientation(EntityPrefab entity, Deviant.Alignment entityAlignment) {
		if (entityAlignment == Deviant.Alignment.Unfriendly)
		{
			entity.setXFlip();
		}
	}
}