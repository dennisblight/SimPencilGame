using System;
using System.Collections.Generic;
using Deanor.Controls;
using Deanor.Structure;

namespace Deanor.AI
{
    public class NormalAI : IGameAI
    {
        public string Name => "Normal AI";

        public IEdge<int> Decision(IGraph<int> g)
        {
            var logicGraph = g.ShallowCopy();
            var p1WinDecisions = new List<IEdge<int>>();
            var safeDecisions = new List<IEdge<int>>();
            foreach (var e in logicGraph.Edges)
            {
                if (e.Cost == 0)
                {
                    e.Cost = 2;
                    var result = logicGraph.CheckWinner(e);
                    switch (result.GameStatus)
                    {
                        case GameStatus.Player1Win:
                            p1WinDecisions.Add(e);
                            break;
                        default:
                            safeDecisions.Add(e);
                            break;
                    }
                    if (result.GameStatus == GameStatus.Player1Win) p1WinDecisions.Add(e);
                    e.Cost = 0;
                }
            }
            var rand = new Random();
            var decisionPool = safeDecisions;
            if (decisionPool.Count == 0)
            {
                decisionPool = p1WinDecisions;
            }
            var decision = decisionPool[rand.Next() % decisionPool.Count];
            return g.FindEdge(decision.VerticeA, decision.VerticeB);
        }
    }
}
