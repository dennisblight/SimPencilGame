using Deanor.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deanor.AI
{
    public interface IGameAI
    {
        IEdge<int> Decision(IGraph<int> g);
        string Name { get; }
    }
}
