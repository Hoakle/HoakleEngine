using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine.Core.Graphics
{
    public interface GraphicalObjectRepresentation<TData>
    {
        public TData Data { get; set; }
    }
}
