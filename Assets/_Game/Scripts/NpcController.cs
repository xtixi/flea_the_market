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
    
    private readonly List<Npc> _npcCharactersOnRoad = new ();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        while (!npcRoad.IsRoadFull())
        {
            CreateNpc();
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
        _npcCharactersOnRoad.Add(createdNpc);
        createdNpc.PickRandomModel();
        createdNpc.Move(npcRoad.GetAvailableSlot());
    }

    public void ReOrderAllNpc()
    {
        _npcCharactersOnRoad.ForEach(x=> x.Move(npcRoad.GetAvailableSlot()));
    }

    public void RemoveNpc(Npc npc)
    {
        npc.Move(spawnPoint);
        _npcCharactersOnRoad.Remove(npc);
        Destroy(npc.gameObject, 5f);
    }
}
