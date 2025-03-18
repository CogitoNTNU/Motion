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

        

    }
}











