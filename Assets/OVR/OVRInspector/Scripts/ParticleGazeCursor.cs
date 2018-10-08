/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;

[RequireComponent(typeof(OVRGazePointer))]
public class ParticleGazeCursor : MonoBehaviour
{
    [Tooltip("Factor effecting particle emmision rate")]
    public float emissionScale;
    [Tooltip("Speed at which max particle emmision is reached")]
    public float maxSpeed;
    [Header("Particle emission curves")]
    // The scale on the x axis of the curves runs from 0 to maxSpeed
    [Tooltip("Curve for trailing edge of pointer")]
    public AnimationCurve halfEmission;

    // These curves are used as a function to map from pointer movement speed to particle emission rate
    [Tooltip("Curve for full perimeter of pointer")]
    public AnimationCurve fullEmission;
    [Tooltip("Curve for full perimeter of pointer")]
    public bool particleTrail;
    [Tooltip("Size of particles")]
    public float particleScale = 0.68f;

    private int sortingOrder = 150;

    MeshRenderer quadRenderer;
    Color particleStartColor;

    OVRGazePointer gazePointer;

    // Use this for initialization
    void Start()
    {
        //Setup references to components
        gazePointer = GetComponent<OVRGazePointer>();
        foreach (Transform child in transform.Find("TrailFollower"))
        {
            if (child.name.Equals("Quad"))
                quadRenderer = child.GetComponent<MeshRenderer>();
        }
        quadRenderer.sortingOrder = sortingOrder;
    }

    
    // Update is called once per frame
    void Update()
    {
        // Use delta to change particle effect based on cursor movement speed
        var delta = GetComponent<OVRGazePointer>().positionDelta;
        
        // Set the main pointers alpha value to the correct level to achieve the desired level of fade
        quadRenderer.material.SetColor("_TintColor",new Color(1, 1, 1, gazePointer.visibilityStrength));
    }
    
}
