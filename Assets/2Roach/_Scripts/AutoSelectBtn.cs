using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AutoSelectBtn : MonoBehaviour
{
    private void Start() {
        GetComponent<Button>().Select();
    }
}
