using System;
using System.Collections.Generic;
using Deanor.Controls;
using Deanor.Structure;

namespace Deanor.AI
{
    public class ProAI : IGameAI
    {
        public string Name => "Pro AI";

        public IEdge<int> Decision(IGraph<int> g)
        {
            var logicGraph = g.ShallowCopy();
            var p1WinDecisions = new List<IEdge<int>>();
            var p2WinDecisions = new List<IEdge<int>>();
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
                    e.Cost = 0;
                }
            }
            foreach (var e in safeDecisions)
            {
                e.Cost = 1;
                var result = logicGraph.CheckWinner(e);
                if (result.GameStatus == GameStatus.Player2Win)
                {
                    p2WinDecisions.Add(e);
                }
                e.Cost = 0;
            }
            foreach (var e in p2WinDecisions)
            {
                safeDecisions.Remove(e);
            }
            var rand = new Random();
            var decisionsPool = safeDecisions;
            if (decisionsPool.Count == 0)
            {
                decisionsPool = p2WinDecisions;
            }
            if (decisionsPool.Count == 0)
            {
                decisionsPool = p1WinDecisions;
            }
            var decision = decisionsPool[rand.Next() % decisionsPool.Count];
            return g.FindEdge(decision.VerticeA, decision.VerticeB);
        }
    }
}
