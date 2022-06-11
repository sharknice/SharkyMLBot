# SharkyMLBot
Machine learning enhancements for the Sharky framework https://github.com/sharknice/Sharky A framework for creating StarCraft 2 bots.

## Usage
Add a reference to the BuildPrediction project and the following code as seen in SharkyMLBot\Program.cs

`var defaultSharkyMLModule = new DefaultSharkyMLModule(defaultSharkyBot);`

`defaultSharkyBot = defaultSharkyMLModule.ActivateMachineLearning(defaultSharkyBot);`

Data for each game is stored as json files in the 'data/ml' folder.  Models are created for each build against each race. Models are also created for specific opponents, but are not used by the included build decision service. 

## Build Decision
The MLBuildDecisionService uses machine learning to determine which build will best counter the last game played against an opponent.

Every build has it's own model and a prediction using that model is made against the data from the last game.  An overall score is given to each build based on the prediction and score of every frame.  The build with the highest score is chosen or it will fall back to the RecentBuildDecisionService if the thresholds aren't met. 

Data is gathered using the MLBuildManager and saved as json in data/ml at the end of every game.

The relevant models are then regenerated using this data with the BuildModelTrainingManager.

## Training
TrainingBot and TrainingBot 2 can be set to play against each other using something like the aiarena ladder manager https://github.com/aiarena/local-play-bootstrap and will gather data for every build.

You can then copy the json files in data/ml over the same folder in SharkyMLBot which will use the data to generate new models.

## Model Tweaking
You can use the MLCSVGenerator project to generate CSVs of your data and usem with the ML.NET Model Builder (BuildPrediction/MLModel1.mbconfig).  Or manually adjust the code in MLModel1.training.cs
