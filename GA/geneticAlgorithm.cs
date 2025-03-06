using System.ComponentModel;

namespace Motion{


    class GeneticAlgorithm{
        public Agent[] population;

        public GeneticAlgorithm(){
            this.population = initializePopulation();
        }
        private Agent initializeAgent(){
            string[] chromosomeNodes = new string[Config.Instance().ChromosomeSize];
            string[] chromosomeEdges = new string[Config.Instance().ChromosomeSize];
            // nodes and edges are initialized here
            for (int i = 0; i < 5; i++)
            {
                chromosomeNodes[i] = "node" + i + ",bias" + i + "=0.5"+ ",activation" + i + "=relu";
            }
            for (int i = 0; i < 5; i++)
            {
                chromosomeEdges[i] = "edge" + i + ", from:"+ 1+ "to:" +i+ ",weight" + i + "=0.5";
            }
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











