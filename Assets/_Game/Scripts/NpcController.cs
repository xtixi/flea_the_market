using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private Npc npcPrefab;
    [SerializeField] private NpcRoad npcRoad;
    [SerializeField] private Transform spawnPoint;
    private List<Npc> _npcCharactersOnRoad = new ();

    private void Start()
    {
        
    }
    


    public void CreateNpc()
    {
        if (npcRoad.IsRoadFull())
        {
            return;
        }
        var createdNpc = Instantiate(npcPrefab,spawnPoint.position,spawnPoint.rotation);
        _npcCharactersOnRoad.Add(createdNpc);
        createdNpc.Move(npcRoad.GetAvailableSlot());
        createdNpc.onResume.AddListener(ReOrderAllNpc);
    }

    public void ReOrderAllNpc()
    {
        _npcCharactersOnRoad.ForEach(x=> x.Move(npcRoad.GetAvailableSlot()));
    }
}
