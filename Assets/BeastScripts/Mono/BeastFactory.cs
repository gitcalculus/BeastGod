// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;

// /// <summary>
// /// </summary>
// public class BeastFactory
// {
//     private static BeastFactory _inst;

//     public static BeastFactory Inst
//     {
//         get
//         {
//             if (null == _inst)
//             {
//                 _inst = new BeastFactory();
//             }

//             return _inst;
//         }
//     }

//     private List<BeastBrain> _beasts;

//     private BeastFactory()
//     {
//         _beasts = new List<BeastBrain>();
//     }

//     public void CreateBeast()
//     {
//     }
// }