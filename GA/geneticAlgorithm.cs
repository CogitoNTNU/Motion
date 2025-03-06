using System.ComponentModel;

namespace Motion{


    class GeneticAlgorithm{
        public Agent[] population;

        public GeneticAlgorithm(){
            this.population = initializePopulation();
        }
        private Agent initializeAgent(){
            string[] chromosomeNodes = new string[10];
            string[] chromosomeEdges = new string[10];
            return new Agent(chromosomeNodes, chromosomeEdges);
        }
        
        private Agent[] initializePopulation(){
            Agent[] population = new Agent[10];
            for (int i = 0; i < 10; i++)
            {
                population[i] = initializeAgent();
            }
            return population;
        }
    }
}











