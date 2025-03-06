namespace Motion
{
    class Config
    {
        // singleton constructor
        private static Config instance = null;
        private int populationSize = 50;
        private int maxGenerations = 100;
        private double mutationRate = 0.01;
        private double crossoverRate = 0.8;
        private int elitismCount = 2;

        private int chromosomeSize = 10;
        private Config() { 
            instance = this;
        }
        public static Config Instance()
        {

            if (instance == null)
            {
                instance = new Config();
            }
            return instance;

        }

        public int PopulationSize
        {
            get { return populationSize; }
            set { populationSize = value; }
        }

        public int MaxGenerations
        {
            get { return maxGenerations; }
            set { maxGenerations = value; }
        }

        public double MutationRate
        {
            get { return mutationRate; }
            set { mutationRate = value; }
        }

        public double CrossoverRate
        {
            get { return crossoverRate; }
            set { crossoverRate = value; }
        }

        public int ElitismCount
        {
            get { return elitismCount; }
            set { elitismCount = value; }
        }

        public int ChromosomeSize
        {
            get { return chromosomeSize; }
            set { chromosomeSize = value; }
        }



        
    }
}