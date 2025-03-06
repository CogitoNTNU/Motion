using System.ComponentModel;

namespace Motion{

    class GeneticAlgorithm {
        public Agent[] population;
        public Config config = Config.Instance();

        public GeneticAlgorithm() {
            this.population = InitializePopulation();
        }
        
        private Agent[] InitializePopulation(){
            Agent[] population = new Agent[config.PopulationSize];
            for (int i = 0; i < config.PopulationSize; i++)
            {
                population[i] = Agent.InitializeAgent(2, 47, 1);    // must add up to population-size
            }
            return population;
        }
    }
}











