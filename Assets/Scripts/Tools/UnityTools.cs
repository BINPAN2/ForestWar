﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public static class UnityTools
    {

        public static Vector3 Parse(string data)
        {
            string[] strs = data.Split(',');
            float x = float.Parse(strs[0]);
            float y = float.Parse(strs[1]);
            float z = float.Parse(strs[2]);
            return new Vector3(x, y, z);
        }
    }
