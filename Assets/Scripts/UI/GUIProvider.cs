using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIProvider : MonoBehaviour
{
    [SerializeField] private GUIView guiView;
    public GUIView GuiView => guiView;

    [SerializeField] private GUIGameOverView guiGameOverView;
    public GUIGameOverView GuiGameOverView => guiGameOverView;
}
