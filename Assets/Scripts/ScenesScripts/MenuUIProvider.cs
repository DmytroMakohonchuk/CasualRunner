using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIProvider : MonoBehaviour
{
    [SerializeField] private MenuUIView menuView;
    public MenuUIView MenuView => menuView;
}
