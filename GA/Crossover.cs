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

        public static Agent Crossover(Agent parent1, Agent parent2)
    {
        // Ensure parent1 has higher or equal fitness
        if (parent2.Fitness > parent1.Fitness)
        {
            var temp = parent1;
            parent1 = parent2;
            parent2 = temp;
        }

        Dictionary<int, EdgeChromosome> edgeDict1 = parent1.Edges.ToDictionary(e => e.InnovationNumber);
        Dictionary<int, EdgeChromosome> edgeDict2 = parent2.Edges.ToDictionary(e => e.InnovationNumber);

        List<EdgeChromosome> childEdges = new List<EdgeChromosome>();

        HashSet<int> allInnovationNumbers = new HashSet<int>(edgeDict1.Keys);
        allInnovationNumbers.UnionWith(edgeDict2.Keys);

        foreach (int innovation in allInnovationNumbers)
        {
            bool has1 = edgeDict1.TryGetValue(innovation, out var gene1);
            bool has2 = edgeDict2.TryGetValue(innovation, out var gene2);

            if (has1 && has2)
            {
                // Matching gene - randomly choose
                var chosen = (new Random().Next(2) == 0) ? gene1 : gene2;
                bool isDisabled = !gene1.Active || !gene2.Active;
                childEdges.Add(new EdgeChromosome(
                    chosen.InnovationNumber,
                    chosen.FromId,
                    chosen.ToId,
                    chosen.Weight)
                {
                    Active = isDisabled ? false : true
                });
            }
            else if (has1)
            {
                // Disjoint/excess gene from more fit parent
                childEdges.Add(new EdgeChromosome(
                    gene1.InnovationNumber,
                    gene1.FromId,
                    gene1.ToId,
                    gene1.Weight)
                {
                    Active = gene1.Active
                });
            }
            // Skip disjoint/excess genes from less fit parent
        }

        // Inherit nodes from the fitter parent
        var childNodes = parent1.Nodes.Select(n =>
            new NodeChromosome(n.Id, n.Bias, n.Activation, n.Type)
            {
                Active = n.Active
            }).ToArray();

        return new Agent(childNodes, childEdges.ToArray());
    }

}