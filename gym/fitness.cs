class fitness{
    public static int fitnessFunction(int stepsize){
        CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); // or AvaloniaEnvViewer.Factory
        bool done = true;
        int fitness = 0;
            for (int i = 0; i < 100_000; i++)
            {
                if (done)
                {
                    NDArray observation = cp.Reset();
                    done = false;
                }
                else
                {
                    var action = (i % 2);
                    var (observation, reward, _done, information) = cp.Step(action); // switching between left and right
                    done = _done;
                    // Do something with the reward and observation.
                    fitness += reward;
                    
                }

                Image img = cp.Render(); // Returns the rendered image.
                Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }
        return fitness;
    }
}

