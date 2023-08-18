using UnityEditor;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AssignMaterial : ScriptableWizard {

    public Material material_to_apply;

    void OnWizardUpdate() {
        helpString = "Select Game Objects";
        isValid = (material_to_apply != null);
    }

    void OnWizardCreate() {
        GameObject[] gos = Selection.gameObjects;
        foreach (GameObject go in gos) {
            RecTrav(go.transform);

            /*Material[] materials = go.GetComponent<Renderer>().sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = material_to_apply;
            go.GetComponent<Renderer>().sharedMaterials = materials;*/

            /*materials = go.GetComponent<Renderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = material_to_apply;
            go.GetComponent<Renderer>().materials = materials;*/


        }

    }

    void RecTrav(Transform parent) {
        foreach (Transform child in parent) {
            if (child.TryGetComponent<Renderer>(out Renderer renderer)) {
                Material[] mats = renderer.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                    mats[i] = material_to_apply;
                renderer.sharedMaterials = mats;
            }
            RecTrav(child);
        }
    }


    [MenuItem("GameObject/Assign Material", false, 4)]
    static void CreateWindow() {
        ScriptableWizard.DisplayWizard("Assign Material", typeof(AssignMaterial), "Assign");
    }
}