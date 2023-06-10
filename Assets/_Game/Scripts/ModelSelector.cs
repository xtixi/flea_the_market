using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ModelSelector : MonoBehaviour
{
    // internal GameObject selectedModel;
    // internal NpcAnimatorController selectedAnimatorController;

    private void Start()
    {
        // InitModelAndAnimator();
    }

    internal Tuple<GameObject, NpcAnimatorController> InitModelAndAnimator()
    {
        var selectedModel = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
        var selectedAnimatorController = selectedModel.GetComponent<NpcAnimatorController>();
        selectedModel.SetActive(true);
        return new Tuple<GameObject, NpcAnimatorController>(selectedModel,selectedAnimatorController);
    }
}
