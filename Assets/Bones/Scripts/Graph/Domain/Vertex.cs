using System.Collections.Generic;

namespace Bones.Scripts.Graph.Domain
{
    public class Vertex<TV,TE>
    {
        public TV element;
        public readonly List<Edge<TV, TE>> outGoing = new List<Edge<TV, TE>>();
        public readonly List<Edge<TV, TE>> inGoing = new List<Edge<TV, TE>>();

        public Vertex(TV element)
        {
            this.element = element;
        }
    }
}