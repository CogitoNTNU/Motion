namespace Motion
{
    class Config
    {
        // singleton constructor
        private static Config instance = null;
        private int populationSize = 50;
        private int numGenerations = 1000;

        public double NodeBiasMutation = 0.8;
        public double NodeBiasSetMutation = 0.1;
        public double ActivationMutation = 0.05;

        public double EdgeWeightMutation = 0.8;
        public double EdgeWeightSetMutation = 0.1;
        public double ToggleEdgeActiveMutation = 0.02;

        public double AddNodeMutation = 0.03;
        public double AddEdgeMutation = 0.05; // you can implement this later
        public double SwapInputEdges = 0.01;
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

        public int NumGenerations
        {
            get { return numGenerations; }
            set { numGenerations = value; }
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