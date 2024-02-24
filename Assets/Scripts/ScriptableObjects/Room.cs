using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu]
    public class Room: ScriptableObject
    {
        public RoomType roomType;

        public Room(RoomType rt)
        {
            roomType = rt;
        }
    }
}
