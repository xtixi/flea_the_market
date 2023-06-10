using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    [SerializeField] private Npc npcPrefab;
    [SerializeField] private NpcRoad npcRoad;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 5f;
    
    private readonly List<Npc> _npcCharactersOnRoad = new ();

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
        createdNpc.onResume.AddListener(ReOrderAllNpc);
    }

    public void ReOrderAllNpc()
    {
        _npcCharactersOnRoad.ForEach(x=> x.Move(npcRoad.GetAvailableSlot()));
    }
}
