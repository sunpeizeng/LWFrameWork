using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TestOdin : SerializedMonoBehaviour
{
   // [NonSerialized, OdinSerialize]
    // Unity will serialize. NOT serialized by Odin.
 //   public IStepTrigger MyReference;
}
