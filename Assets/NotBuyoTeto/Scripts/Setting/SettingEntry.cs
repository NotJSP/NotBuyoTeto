﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotBuyoTeto.UI;

namespace NotBuyoTeto.Setting {
    public abstract class SettingEntry : MonoBehaviour {
        [SerializeField] protected UITemplates templates;

        public abstract void Create(RectTransform container);
    }
}
