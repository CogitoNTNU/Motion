namespace Motion{

    class Crossover{
        public static bool IsNotEdgeInChromosome(EdgeChromosome edge, EdgeChromosome[] chromosomeEdges){
            foreach (EdgeChromosome chromosomeEdge in chromosomeEdges)
            {
                if (edge.FromId == chromosomeEdge.FromId && edge.ToId == chromosomeEdge.ToId)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNotNodeInChromosome(NodeChromosome node, NodeChromosome[] chromosomeNodes){
            foreach (NodeChromosome chromosomeNode in chromosomeNodes)
            {
                if (node.Id == chromosomeNode.Id)
                {
                    return false;
                }
            }
            return true;
        }
    
        public static void addNodeToChromosome(Agent parent, NodeChromosome[] childChromosomeNodes, int nodeId){
            int edgeChromosomeLength = parent.Edges.Length;

            NodeChromosome node = parent.GetNodeChromosomeFromId(nodeId);
            if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
            }    
        }

        public static Agent dominantCrossover(Agent parent1, Agent parent2){
            
            // Get max chromosome length
            Agent dominant = parent1.Fitness > parent2.Fitness ? parent1 : parent2;
            Agent notDominant = parent1.Fitness > parent2.Fitness ? parent2 : parent1;
            int edgeChromosomeLength = dominant.Edges.Length > notDominant.Edges.Length ? dominant.Edges.Length : notDominant.Edges.Length;

            // Crossover nodes
            NodeChromosome[] childChromosomeNodes = new NodeChromosome[0];
            EdgeChromosome[] childChromosomeEdges = new EdgeChromosome[0];

            int index = 0;

            for (int i = 0; i < edgeChromosomeLength; i++ ){
                if (IsNotEdgeInChromosome(dominant.Edges[i], childChromosomeEdges) && i < dominant.Edges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, dominant.Edges[i].FromId, dominant.Edges[i].ToId, dominant.Edges[i].Weight));
                    index++;
            
                    addNodeToChromosome(dominant, childChromosomeNodes, dominant.Edges[i].FromId);
                    addNodeToChromosome(dominant, childChromosomeNodes, dominant.Edges[i].ToId);
                    

                }else if (IsNotEdgeInChromosome(notDominant.Edges[i], childChromosomeEdges) && i < notDominant.Edges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, notDominant.Edges[i].FromId, notDominant.Edges[i].ToId, notDominant.Edges[i].Weight));
                    index++;

                    addNodeToChromosome(notDominant, childChromosomeNodes, notDominant.Edges[i].FromId);
                    addNodeToChromosome(notDominant, childChromosomeNodes, notDominant.Edges[i].ToId);
                }
            }   

            Agent child = new Agent(childChromosomeNodes, childChromosomeEdges);

            return child;
        }

        public static Agent DoCrossover(Agent parent1, Agent parent2){

            // Get max chromosome length
            Agent dominant = parent1.Fitness > parent2.Fitness ? parent1 : parent2;
            Agent notDominant = parent1.Fitness > parent2.Fitness ? parent2 : parent1;
            int edgeChromosomeLength = dominant.Edges.Length > notDominant.Edges.Length ? dominant.Edges.Length : notDominant.Edges.Length;

            // Crossover nodes
            NodeChromosome[] childChromosomeNodes = new NodeChromosome[0];
            EdgeChromosome[] childChromosomeEdges = new EdgeChromosome[0];
            int index = 0;
      
            for (int i = 0; i < edgeChromosomeLength; i++)
            {
                
                if (IsNotEdgeInChromosome(dominant.Edges[i], childChromosomeEdges) && i < dominant.Edges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, dominant.Edges[i].FromId, dominant.Edges[i].ToId, dominant.Edges[i].Weight));
                    index++;
                    int nodeId = dominant.Edges[i].FromId;
                    NodeChromosome node = dominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }
                    nodeId = dominant.Edges[i].ToId;
                    node = dominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }

                } else if (IsNotEdgeInChromosome(notDominant.Edges[i], childChromosomeEdges) && i < notDominant.Edges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, notDominant.Edges[i].FromId, notDominant.Edges[i].ToId, notDominant.Edges[i].Weight));
                    index++;
                    int nodeId = notDominant.Edges[i].FromId;
                    NodeChromosome node = notDominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }
                    nodeId = notDominant.Edges[i].ToId;
                    node = notDominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }

                }                
                
            }

            Agent child = new Agent(childChromosomeNodes, childChromosomeEdges); 

            return child;
            
        } 

        

        public static Agent newCrossover(Agent parent1, Agent parent2){
            
            Agent dominant = parent1.AdjustedFitness > parent2.AdjustedFitness ? parent1 : parent2;
            Agent notDominant = parent1.AdjustedFitness > parent2.AdjustedFitness ? parent2 : parent1;
            
            EdgeChromosome[] childChromosomeEdges = new EdgeChromosome[0];
            
            EdgeChromosome[] dominantEdges  = dominant.Edges;
            EdgeChromosome[] nonDominantEdges  = notDominant.Edges;

            double fitnessSum = parent1.AdjustedFitness + parent2.AdjustedFitness;
            double probabilityFromParent1 = parent1.AdjustedFitness / fitnessSum;

            Random rand = new Random();

            foreach (EdgeChromosome edge1 in dominantEdges) {
                foreach (EdgeChromosome edge2 in nonDominantEdges) {
                    if (edge1.InnovationNumber == edge2.InnovationNumber) {
                        
                        EdgeChromosome chosenEdge = rand.NextDouble() < probabilityFromParent1 ? edge1 : edge2;
                        childChromosomeEdges.Append(chosenEdge);
                        break;
        
                    }
                }
            }
            
            // TODO: add disjoint and excess genes to the child


            return child;
        }

    }




}