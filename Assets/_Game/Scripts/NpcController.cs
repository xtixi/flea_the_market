using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    public static NpcController instance;
    
    [SerializeField] private Npc npcPrefab;
    [SerializeField] private NpcRoad npcRoad;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 5f;
    
    public readonly List<Npc> npcCharactersOnRoad = new ();
    public bool isSpawningEnded;
    
    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        isSpawningEnded = false;
        var randomNumber = Random.Range(2, npcRoad.slots.Count);
        var count = 0;
        while (!npcRoad.IsRoadFull() && count < randomNumber)
        {
            count++;
            CreateNpc();
            if (count >= randomNumber)
            {
                isSpawningEnded = true;
            }
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }

    }


    public void CreateNpc()
    {
        if (npcRoad.IsRoadFull())
        {
            return;
        }
        var createdNpc = Instantiate(npcPrefab,spawnPoint.position,spawnPoint.rotation);
        npcCharactersOnRoad.Add(createdNpc);
        createdNpc.PickRandomModel();
        createdNpc.Move(npcRoad.GetAvailableSlot());
    }

    public void ReOrderAllNpc()
    {
        npcCharactersOnRoad.ForEach(x=> x.Move(npcRoad.GetAvailableSlot()));
    }

    public void RemoveNpc(Npc npc)
    {
        npc.Move(spawnPoint);
        npcCharactersOnRoad.Remove(npc);
        Destroy(npc.gameObject, 5f);
        GameUIController.instance.InitItemValues(null);
        if (npcCharactersOnRoad.Count <= 0 && isSpawningEnded)
        {
            GameController.instance.EndDay();
        }
    }
}
