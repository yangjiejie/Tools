﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


namespace Psd
{
    public interface IImageImport
    {
        void DrawImage(PSImage image, GameObject parent, GameObject ownObj = null);
    }
}
