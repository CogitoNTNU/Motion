


namespace Motion
{
    class Program
    {
        static void Main(string[] args)
        {
            // GymRunner.RunGymEnvironment();
            MyRLExperiment experiment = new MyRLExperiment();
            experiment.RunPPO(useCuda: false); 
        }
    }
}