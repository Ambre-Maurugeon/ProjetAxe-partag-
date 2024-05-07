using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenVisibility : MonoBehaviour
{
    [Header("Objets visibles")]
    public Transform objet01Transform;
    public Transform objet02Transform;

    [Header("Camera")]
    public Camera mainCamera;

    [HideInInspector]
    public bool OnScreen=false;

    void Update()
    {
        // Convertir les positions en coordonnées de vue
        Vector3 objet01ViewportPos = mainCamera.WorldToViewportPoint(objet01Transform.position);
        Vector3 objet02ViewportPos = mainCamera.WorldToViewportPoint(objet02Transform.position);

        // Vérif si objets sont à l'écran
        bool objet01OnScreen = IsInViewport(objet01ViewportPos);
        bool objet02OnScreen = IsInViewport(objet02ViewportPos);

        if (objet01OnScreen && objet02OnScreen)
        {
            //Debug.Log("Les deux objets sont à l'écran ");
            OnScreen = true;
        }
        else{
            OnScreen = false;
        }
    }

    bool IsInViewport(Vector3 viewportPos)
    {
        // si 0<x<1 et 0<y<1 = OnScreen
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }
}
