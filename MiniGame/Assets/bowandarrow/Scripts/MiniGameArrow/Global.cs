using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGameArrow
{
    
    public class Global
    {
        public static bool autoShoot = false;
        public static bool needControlResult = false;

        public static float flyTime = 0f;
        public static float power = 0f;
        public static List<ShootData> dataList = new List<ShootData>();
        public static List<ShootData> dataListFromConfig = new List<ShootData>();
    }
    
}

