using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Psd
{
    public class Layer
    {
        public string name;
        public LayerType type;
        public Layer[] layers;
        public string[] arguments;
        //public PSImage[] images;
        public PSImage image;
        public Size size;
        public Position position;
    }
}

