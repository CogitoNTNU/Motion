using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Avalonia.OpenGL.Angle;

namespace Motion{

    class GeneticAlgorithm {
        public Agent[] population;
        public Config config = Config.Instance();

        public GeneticAlgorithm() {
            this.population = InitializePopulation();
        }
        
        public Agent[] InitializePopulation(){
            Agent[] population = new Agent[config.PopulationSize];
            for (int i = 0; i < config.PopulationSize; i++)
            {
                population[i] = Agent.InitializeAgent(2, 47, 1);    // must add up to population-size
            }
            return population;
        }

        private bool IsNotEdgeInChromosome(EdgeChromosome edge, EdgeChromosome[] chromosomeEdges){
            foreach (EdgeChromosome chromosomeEdge in chromosomeEdges)
            {
                if (edge.FromId == chromosomeEdge.FromId && edge.ToId == chromosomeEdge.ToId)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsNotNodeInChromosome(NodeChromosome node, NodeChromosome[] chromosomeNodes){
            foreach (NodeChromosome chromosomeNode in chromosomeNodes)
            {
                if (node.Id == chromosomeNode.Id)
                {
                    return false;
                }
            }
            return true;
        }

        private Agent Crossover(Agent parent1, Agent parent2){
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











