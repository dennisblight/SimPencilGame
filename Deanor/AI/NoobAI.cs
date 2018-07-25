using System;
using System.Linq;
using Deanor.Structure;

namespace Deanor.AI
{
    public class NoobAI : IGameAI
    {
        public string Name => "Noob AI";

        public IEdge<int> Decision(IGraph<int> g)
        {
            var decisions = (from edge in g.Edges where edge.Cost == 0 select edge).ToArray();
            var rand = new Random();
            return decisions[rand.Next() % decisions.Length];
        }
    }
}
