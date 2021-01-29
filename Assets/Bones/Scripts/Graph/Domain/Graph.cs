
using System.Collections.Generic;

namespace Bones.Scripts.Graph.Domain
{
    public interface Graph<TV,TE>
    {
        void Clear();
        IReadOnlyList<Vertex<TV,TE>> Vertices { get; }
        IReadOnlyList<Edge<TV,TE>> Edges { get; }
        Vertex<TV, TE> AddVertex(TV element);
        
        Vertex<TV, TE> AddUniqueVertex(TV element);
        Edge<TV, TE> AddEdge(TE element, Vertex<TV,TE> previous, Vertex<TV,TE> next);

        void RemoveEdge(Edge<TV, TE> edge);
        void RemoveVertex(Vertex<TV, TE> vertex);
    }
}