namespace Motion{

    class Mutate{

        /// <summary>
        /// Clamp a number between an upper and lower bound.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="upperBound"></param>
        /// <param name="lowerBound"></param>
        /// <returns>clamped value (double)</returns>
        public static double Clamp(double num, double upperBound = 1, double lowerBound = -1){
            if (num > upperBound){
                return upperBound;
            }
            else if (num < lowerBound){
                return lowerBound;
            }
            else{
                return num;
            }
        }

        public static void MutateNodeBias(NodeChromosome node, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                node.Bias = Clamp(node.Bias + (Random.NextDouble() * 2 - 1));
            }
        }

        public static void MutateSetNodeBias(NodeChromosome node, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                
                node.Bias = Clamp(Random.NextDouble() * 2 - 1);
            }
        }



        public static void MutateEdgeWeight(EdgeChromosome edge, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                edge.Weight = Clamp(edge.Weight + (Random.NextDouble() * 2 - 1));
            }
        }

        public static void MutateSetEdgeWeight(EdgeChromosome edge, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                edge.Weight = Clamp(Random.NextDouble() * 2 - 1);
            }
        }

        public static void MutateAllEdgeWeight(EdgeChromosome[] edges, double mutationRate){
            Random Random = new Random();
            foreach (EdgeChromosome edge in edges)
            {
                if (Random.NextDouble() < mutationRate){
                    edge.Weight = Clamp(edge.Weight + (Random.NextDouble() * 2 - 1));
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

                edge.Weight = 0.5;
                edge.ToId = newNode.Id;

                agent.AddNode(newNode);
                agent.AddEdge(newEdge);

            }
        }


    
    }

}