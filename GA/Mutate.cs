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

        public static void MutateActivation(NodeChromosome node, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                // TODO: Implement activation function mutation
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

        public static void SwapEdgesFromInput(Agent agent, double mutationRate){
            Random Random = new Random();
            if (Random.NextDouble() < mutationRate){
                EdgeChromosome[] edges = agent.Edges;
                EdgeChromosome[] swapCandidates = [];
                foreach (EdgeChromosome edge in edges)
                {
                    // find all edges that have an input node as the from node
                    if (agent.GetNodeChromosomeFromId(edge.FromId).Type == NodeType.Input){
                        swapCandidates.Append(edge);
                    } 
                }
                // choose two random fromIDs from the swapCandidates
                int index1 = Random.Next(0, swapCandidates.Length);
                int id1 = swapCandidates[index1].FromId;
                int id2 = id1;
                while (id2 == id1){
                    int index2 = Random.Next(0, swapCandidates.Length);
                    id2 = swapCandidates[index2].FromId;
                }
                // swap the edges
                foreach (EdgeChromosome edge in edges)
                {
                    if (edge.FromId == id1){
                        edge.FromId = id2;
                    }
                    else if (edge.FromId == id2){
                        edge.FromId = id1;
                    }
                }
              
            }
        }


    
    }

}