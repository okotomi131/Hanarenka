using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
