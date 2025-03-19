namespace Motion{

    class Mutate{


        public static void MutateNodeBias(NodeChromosome node, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                node.Bias = node.Bias + (Random.NextDouble() * 2 - 1);
            }
        }

        public static void MutateEdgeWeight(EdgeChromosome edge, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                edge.Weight = edge.Weight + (Random.NextDouble() * 2 - 1);
            }
        }

        public static void MutateEdgeWeight(EdgeChromosome[] edges, double mutationRate){
            Random Random = new Random();
            foreach (EdgeChromosome edge in edges)
            {
                if (Random.NextDouble() < mutationRate){
                    edge.Weight = edge.Weight + (Random.NextDouble() * 2 - 1);
                }
            }
        }

        public static void MutateEdgeActiveStatus(EdgeChromosome edge, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                edge.Active = !edge.Active;
            }
        }

        public static void MutateNewNode(EdgeChromosome edge, Agent agent, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){

                int nodeCount = agent.Nodes.Length;
                int edgeCount = agent.Edges.Length;

                NodeChromosome newNode = new NodeChromosome(nodeCount, 0.5, "relu", NodeType.Hidden);
                EdgeChromosome newEdge = new EdgeChromosome(edgeCount, newNode.Id, edge.ToId, 1);

                edge.Weight = 1;
                edge.ToId = newNode.Id;

                agent.AddNode(newNode);
                agent.AddEdge(newEdge);

            }
        }


    
    }

}