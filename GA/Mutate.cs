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

                int nodeCount = agent.Nodes.Count;
                int edgeCount = agent.Edges.Count;

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
                List<EdgeChromosome> edges = agent.Edges;
                List<EdgeChromosome> swapCandidates = new List<EdgeChromosome>();
                foreach (EdgeChromosome edge in edges)
                {
                    // find all edges that have an input node as the from node
                    if (agent.GetNodeChromosomeFromId(edge.FromId).Type == NodeType.Input){
                        swapCandidates.Append(edge);
                    } 
                }
                // choose two random fromIDs from the swapCandidates
                int index1 = Random.Next(0, swapCandidates.Count);
                int id1 = swapCandidates[index1].FromId;
                int id2 = id1;
                while (id2 == id1){
                    int index2 = Random.Next(0, swapCandidates.Count);
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


        public static void AddEdgeMutation(Agent agent, double mutationRate)
        {
            Random random = new Random();

            if (random.NextDouble() >= mutationRate)
                return;

            var nodes = agent.Nodes;
            var edges = agent.Edges;

            var nodeList = nodes.Where(n => n.Type != NodeType.Output).ToList();
            var targetList = nodes.Where(n => n.Type != NodeType.Input).ToList();

            if (nodeList.Count == 0 || targetList.Count == 0)
                return;

            var fromNode = nodeList[random.Next(nodeList.Count)];
            var toNode = targetList[random.Next(targetList.Count)];

            // prevent loops and duplicate connections
            if (fromNode.Id == toNode.Id || edges.Any(e => e.FromId == fromNode.Id && e.ToId == toNode.Id))
                return;

            int innovation = Innovation.GetInstance().GetInnovationNumber(fromNode.Id, toNode.Id);
            var newEdge = new EdgeChromosome(innovation, fromNode.Id, toNode.Id, Clamp(random.NextDouble() * 2 - 1));
            agent.AddEdge(newEdge);
        }



        public static void ApplyMutations(Agent agent)
        {
            Random random = new Random();

            foreach (var node in agent.Nodes)
            {
                MutateNodeBias(node, Config.Instance().NodeBiasMutation);
                MutateSetNodeBias(node, Config.Instance().NodeBiasSetMutation);
                MutateActivation(node, Config.Instance().ActivationMutation);
            }

            foreach (var edge in agent.Edges)
            {
                MutateEdgeWeight(edge, Config.Instance().EdgeWeightMutation);
                MutateSetEdgeWeight(edge, Config.Instance().EdgeWeightSetMutation);
                MutateEdgeActiveStatus(edge, Config.Instance().ToggleEdgeActiveMutation);
                MutateNewNode(edge, agent, Config.Instance().AddNodeMutation);
            }

            AddEdgeMutation(agent, Config.Instance().AddEdgeMutation);
            SwapEdgesFromInput(agent, Config.Instance().SwapInputEdges);
        }



    
    }

}