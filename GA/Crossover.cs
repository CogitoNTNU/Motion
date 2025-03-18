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
            int edgeChromosomeLength = parent.chromosomeEdges.Length;

            NodeChromosome node = parent.GetNodeChromosomeFromId(nodeId);
            if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
            }    
        }

        public static Agent dominantCrossover(Agent parent1, Agent parent2){
            Agent child = new Agent();
            
            // Get max chromosome length
            Agent dominant = parent1.fitness > parent2.fitness ? parent1 : parent2;
            Agent notDominant = parent1.fitness > parent2.fitness ? parent2 : parent1;
            int edgeChromosomeLength = dominant.chromosomeEdges.Length > notDominant.chromosomeEdges.Length ? dominant.chromosomeEdges.Length : notDominant.chromosomeEdges.Length;

            // Crossover nodes
            NodeChromosome[] childChromosomeNodes = new NodeChromosome[0];
            EdgeChromosome[] childChromosomeEdges = new EdgeChromosome[0];

            int index = 0;

            for (int i = 0; i < edgeChromosomeLength; i++ ){
                if (IsNotEdgeInChromosome(dominant.chromosomeEdges[i], childChromosomeEdges) && i < dominant.chromosomeEdges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, dominant.chromosomeEdges[i].FromId, dominant.chromosomeEdges[i].ToId, dominant.chromosomeEdges[i].Weight));
                    index++;
            
                    addNodeToChromosome(dominant, childChromosomeNodes, dominant.chromosomeEdges[i].FromId);
                    addNodeToChromosome(dominant, childChromosomeNodes, dominant.chromosomeEdges[i].ToId);
                    

                }else if (IsNotEdgeInChromosome(notDominant.chromosomeEdges[i], childChromosomeEdges) && i < notDominant.chromosomeEdges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, notDominant.chromosomeEdges[i].FromId, notDominant.chromosomeEdges[i].ToId, notDominant.chromosomeEdges[i].Weight));
                    index++;

                    addNodeToChromosome(notDominant, childChromosomeNodes, notDominant.chromosomeEdges[i].FromId);
                    addNodeToChromosome(notDominant, childChromosomeNodes, notDominant.chromosomeEdges[i].ToId);
                }
            }
            child.chromosomeNodes = childChromosomeNodes;
            child.chromosomeEdges = childChromosomeEdges;   
            
            return child;
        }

        public static Agent DoCrossover(Agent parent1, Agent parent2){
            Agent child = new Agent();
            
           
            // Get max chromosome length
            Agent dominant = parent1.fitness > parent2.fitness ? parent1 : parent2;
            Agent notDominant = parent1.fitness > parent2.fitness ? parent2 : parent1;
            int edgeChromosomeLength = dominant.chromosomeEdges.Length > notDominant.chromosomeEdges.Length ? dominant.chromosomeEdges.Length : notDominant.chromosomeEdges.Length;

            // Crossover nodes
            NodeChromosome[] childChromosomeNodes = new NodeChromosome[0];
            EdgeChromosome[] childChromosomeEdges = new EdgeChromosome[0];
            int index = 0;
      
            for (int i = 0; i < edgeChromosomeLength; i++)
            {
                
                if (IsNotEdgeInChromosome(dominant.chromosomeEdges[i], childChromosomeEdges) && i < dominant.chromosomeEdges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, dominant.chromosomeEdges[i].FromId, dominant.chromosomeEdges[i].ToId, dominant.chromosomeEdges[i].Weight));
                    index++;
                    int nodeId = dominant.chromosomeEdges[i].FromId;
                    NodeChromosome node = dominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }
                    nodeId = dominant.chromosomeEdges[i].ToId;
                    node = dominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }

                }else if (IsNotEdgeInChromosome(notDominant.chromosomeEdges[i], childChromosomeEdges) && i < notDominant.chromosomeEdges.Length){
                    childChromosomeEdges.Append(new EdgeChromosome(index, notDominant.chromosomeEdges[i].FromId, notDominant.chromosomeEdges[i].ToId, notDominant.chromosomeEdges[i].Weight));
                    index++;
                    int nodeId = notDominant.chromosomeEdges[i].FromId;
                    NodeChromosome node = notDominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }
                    nodeId = notDominant.chromosomeEdges[i].ToId;
                    node = notDominant.GetNodeChromosomeFromId(nodeId);
                    if (node != null && IsNotNodeInChromosome(node, childChromosomeNodes)){
                        childChromosomeNodes.Append(new NodeChromosome(nodeId, node.Bias, node.Activation, node.Type));
                    }

                }                
                
            }
            child.chromosomeNodes = childChromosomeNodes;
            child.chromosomeEdges = childChromosomeEdges;   
            
            return child;
        }

        



    }




}