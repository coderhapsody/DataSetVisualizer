using Allegro.Visualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Allegro.Visualizer.NetCore.DebuggeeSide
{
    public class ListVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var origObject = target as IList;
            var serializableModel = new SerializableModel(origObject);
            StreamSerializer.ObjectToStream(outgoingData, serializableModel);
        }
    }

    [Serializable]
    public class SerializableModel : ArrayList
    {
        public SerializableModel(IList listObject)
        {
            foreach(object item in listObject)
            {
                Add(item);
            }
        }        

    }
}
