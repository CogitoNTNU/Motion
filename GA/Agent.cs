namespace Motion{
    class Agent{
        public string[] chromosomeNodes;
        public string[] chromosomeEdges;
        public double fitness;
        public double adjustedFitness;

        public Agent(string[] chromosomeNodes, string[] chromosomeEdges){
            this.chromosomeNodes = chromosomeNodes;
            this.chromosomeEdges = chromosomeEdges;
            this.fitness = 0.0;
            this.adjustedFitness = 0.0;
        }
    }
}