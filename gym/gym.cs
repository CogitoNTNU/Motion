



// using NumSharp;
// using SixLabors.ImageSharp;
// using Gym.Environments;
// using Gym.Environments.Envs.Classic;
// using Gym.Rendering.WinForm;

// // https://github.com/SciSharp/Gym.NET

// CartPoleEnv cp = new CartPoleEnv(WinFormEnvViewer.Factory); //or AvaloniaEnvViewer.Factory
// bool done = true;
// for (int i = 0; i < 100_000; i++)
// {
//     if (done)
//     {
//         NDArray observation = cp.Reset();
//         done = false;
//     }
//     else
//     {
//         var (observation, reward, _done, information) = cp.Step((i % 2)); //we switch between moving left and right
//         done = _done;
//         //do something with the reward and observation.
//     }

//     SixLabors.ImageSharp.Image img = cp.Render(); //returns the image that was rendered.
//     Thread.Sleep(15); //this is to prevent it from finishing instantly !
// }

// cp.Close();