﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers;
using Microsoft.ML;

namespace SharkyMLBot
{
    public partial class MLModel1
    {
        public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(new []{new InputOutputColumnPair(@"ProxyDetected", @"ProxyDetected"),new InputOutputColumnPair(@"MapName", @"MapName"),new InputOutputColumnPair(@"ProxyActive", @"ProxyActive")})      
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"ENEMY_PROTOSS_IMMORTAL", @"ENEMY_PROTOSS_IMMORTAL"),new InputOutputColumnPair(@"ENEMY_PROTOSS_HIGHTEMPLAR", @"ENEMY_PROTOSS_HIGHTEMPLAR"),new InputOutputColumnPair(@"ENEMY_PROTOSS_GATEWAY", @"ENEMY_PROTOSS_GATEWAY"),new InputOutputColumnPair(@"ENEMY_PROTOSS_FORGE", @"ENEMY_PROTOSS_FORGE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_FLEETBEACON", @"ENEMY_PROTOSS_FLEETBEACON"),new InputOutputColumnPair(@"ENEMY_PROTOSS_DISRUPTORPHASED", @"ENEMY_PROTOSS_DISRUPTORPHASED"),new InputOutputColumnPair(@"ENEMY_PROTOSS_DISRUPTOR", @"ENEMY_PROTOSS_DISRUPTOR"),new InputOutputColumnPair(@"ENEMY_PROTOSS_DARKTEMPLAR", @"ENEMY_PROTOSS_DARKTEMPLAR"),new InputOutputColumnPair(@"ENEMY_PROTOSS_DARKSHRINE", @"ENEMY_PROTOSS_DARKSHRINE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_INTERCEPTOR", @"ENEMY_PROTOSS_INTERCEPTOR"),new InputOutputColumnPair(@"ENEMY_PROTOSS_CYBERNETICSCORE", @"ENEMY_PROTOSS_CYBERNETICSCORE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_CARRIER", @"ENEMY_PROTOSS_CARRIER"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ASSIMILATOR", @"ENEMY_PROTOSS_ASSIMILATOR"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ARCHON", @"ENEMY_PROTOSS_ARCHON"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ADEPTPHASESHIFT", @"ENEMY_PROTOSS_ADEPTPHASESHIFT"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ADEPT", @"ENEMY_PROTOSS_ADEPT"),new InputOutputColumnPair(@"SELF_PROTOSS_ASSIMILATORRICH", @"SELF_PROTOSS_ASSIMILATORRICH"),new InputOutputColumnPair(@"SELF_PROTOSS_ZEALOT", @"SELF_PROTOSS_ZEALOT"),new InputOutputColumnPair(@"SELF_PROTOSS_WARPPRISMPHASING", @"SELF_PROTOSS_WARPPRISMPHASING"),new InputOutputColumnPair(@"SELF_PROTOSS_WARPPRISM", @"SELF_PROTOSS_WARPPRISM"),new InputOutputColumnPair(@"ENEMY_PROTOSS_COLOSSUS", @"ENEMY_PROTOSS_COLOSSUS"),new InputOutputColumnPair(@"ENEMY_PROTOSS_MOTHERSHIP", @"ENEMY_PROTOSS_MOTHERSHIP"),new InputOutputColumnPair(@"ENEMY_PROTOSS_MOTHERSHIPCORE", @"ENEMY_PROTOSS_MOTHERSHIPCORE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_NEXUS", @"ENEMY_PROTOSS_NEXUS"),new InputOutputColumnPair(@"ENEMY_PROTOSS_WARPPRISMPHASING", @"ENEMY_PROTOSS_WARPPRISMPHASING"),new InputOutputColumnPair(@"ENEMY_PROTOSS_WARPPRISM", @"ENEMY_PROTOSS_WARPPRISM"),new InputOutputColumnPair(@"ENEMY_PROTOSS_WARPGATE", @"ENEMY_PROTOSS_WARPGATE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_VOIDRAY", @"ENEMY_PROTOSS_VOIDRAY"),new InputOutputColumnPair(@"ENEMY_PROTOSS_TWILIGHTCOUNCIL", @"ENEMY_PROTOSS_TWILIGHTCOUNCIL"),new InputOutputColumnPair(@"ENEMY_PROTOSS_TEMPLARARCHIVE", @"ENEMY_PROTOSS_TEMPLARARCHIVE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_TEMPEST", @"ENEMY_PROTOSS_TEMPEST"),new InputOutputColumnPair(@"ENEMY_PROTOSS_STARGATE", @"ENEMY_PROTOSS_STARGATE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_STALKER", @"ENEMY_PROTOSS_STALKER"),new InputOutputColumnPair(@"ENEMY_PROTOSS_SHIELDBATTERY", @"ENEMY_PROTOSS_SHIELDBATTERY"),new InputOutputColumnPair(@"ENEMY_PROTOSS_SENTRY", @"ENEMY_PROTOSS_SENTRY"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ROBOTICSFACILITY", @"ENEMY_PROTOSS_ROBOTICSFACILITY"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ROBOTICSBAY", @"ENEMY_PROTOSS_ROBOTICSBAY"),new InputOutputColumnPair(@"ENEMY_PROTOSS_PYLONOVERCHARGED", @"ENEMY_PROTOSS_PYLONOVERCHARGED"),new InputOutputColumnPair(@"ENEMY_PROTOSS_PYLON", @"ENEMY_PROTOSS_PYLON"),new InputOutputColumnPair(@"ENEMY_PROTOSS_PROBE", @"ENEMY_PROTOSS_PROBE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_PHOTONCANNON", @"ENEMY_PROTOSS_PHOTONCANNON"),new InputOutputColumnPair(@"ENEMY_PROTOSS_PHOENIX", @"ENEMY_PROTOSS_PHOENIX"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ORACLESTASISTRAP", @"ENEMY_PROTOSS_ORACLESTASISTRAP"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ORACLE", @"ENEMY_PROTOSS_ORACLE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_OBSERVER", @"ENEMY_PROTOSS_OBSERVER"),new InputOutputColumnPair(@"SELF_PROTOSS_WARPGATE", @"SELF_PROTOSS_WARPGATE"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ZEALOT", @"ENEMY_PROTOSS_ZEALOT"),new InputOutputColumnPair(@"SELF_PROTOSS_VOIDRAY", @"SELF_PROTOSS_VOIDRAY"),new InputOutputColumnPair(@"SELF_PROTOSS_TEMPLARARCHIVE", @"SELF_PROTOSS_TEMPLARARCHIVE"),new InputOutputColumnPair(@"SELF_PROTOSS_DISRUPTOR", @"SELF_PROTOSS_DISRUPTOR"),new InputOutputColumnPair(@"SELF_PROTOSS_DARKTEMPLAR", @"SELF_PROTOSS_DARKTEMPLAR"),new InputOutputColumnPair(@"SELF_PROTOSS_DARKSHRINE", @"SELF_PROTOSS_DARKSHRINE"),new InputOutputColumnPair(@"SELF_PROTOSS_CYBERNETICSCORE", @"SELF_PROTOSS_CYBERNETICSCORE"),new InputOutputColumnPair(@"SELF_PROTOSS_COLOSSUS", @"SELF_PROTOSS_COLOSSUS"),new InputOutputColumnPair(@"SELF_PROTOSS_CARRIER", @"SELF_PROTOSS_CARRIER"),new InputOutputColumnPair(@"SELF_PROTOSS_ASSIMILATOR", @"SELF_PROTOSS_ASSIMILATOR"),new InputOutputColumnPair(@"SELF_PROTOSS_ARCHON", @"SELF_PROTOSS_ARCHON"),new InputOutputColumnPair(@"SELF_PROTOSS_ADEPTPHASESHIFT", @"SELF_PROTOSS_ADEPTPHASESHIFT"),new InputOutputColumnPair(@"SELF_PROTOSS_DISRUPTORPHASED", @"SELF_PROTOSS_DISRUPTORPHASED"),new InputOutputColumnPair(@"SELF_PROTOSS_ADEPT", @"SELF_PROTOSS_ADEPT"),new InputOutputColumnPair(@"Frame", @"Frame"),new InputOutputColumnPair(@"MyRace", @"MyRace"),new InputOutputColumnPair(@"MySelectedRace", @"MySelectedRace"),new InputOutputColumnPair(@"EnemyRace", @"EnemyRace"),new InputOutputColumnPair(@"EnemySelectedRace", @"EnemySelectedRace"),new InputOutputColumnPair(@"SELF_PROTOSS_FLEETBEACON", @"SELF_PROTOSS_FLEETBEACON"),new InputOutputColumnPair(@"SELF_PROTOSS_FORGE", @"SELF_PROTOSS_FORGE"),new InputOutputColumnPair(@"SELF_PROTOSS_GATEWAY", @"SELF_PROTOSS_GATEWAY"),new InputOutputColumnPair(@"SELF_PROTOSS_TEMPEST", @"SELF_PROTOSS_TEMPEST"),new InputOutputColumnPair(@"SELF_PROTOSS_STARGATE", @"SELF_PROTOSS_STARGATE"),new InputOutputColumnPair(@"SELF_PROTOSS_STALKER", @"SELF_PROTOSS_STALKER"),new InputOutputColumnPair(@"SELF_PROTOSS_SHIELDBATTERY", @"SELF_PROTOSS_SHIELDBATTERY"),new InputOutputColumnPair(@"SELF_PROTOSS_SENTRY", @"SELF_PROTOSS_SENTRY"),new InputOutputColumnPair(@"SELF_PROTOSS_ROBOTICSFACILITY", @"SELF_PROTOSS_ROBOTICSFACILITY"),new InputOutputColumnPair(@"SELF_PROTOSS_ROBOTICSBAY", @"SELF_PROTOSS_ROBOTICSBAY"),new InputOutputColumnPair(@"SELF_PROTOSS_PYLONOVERCHARGED", @"SELF_PROTOSS_PYLONOVERCHARGED"),new InputOutputColumnPair(@"SELF_PROTOSS_PYLON", @"SELF_PROTOSS_PYLON"),new InputOutputColumnPair(@"SELF_PROTOSS_PROBE", @"SELF_PROTOSS_PROBE"),new InputOutputColumnPair(@"SELF_PROTOSS_PHOTONCANNON", @"SELF_PROTOSS_PHOTONCANNON"),new InputOutputColumnPair(@"SELF_PROTOSS_PHOENIX", @"SELF_PROTOSS_PHOENIX"),new InputOutputColumnPair(@"SELF_PROTOSS_ORACLESTASISTRAP", @"SELF_PROTOSS_ORACLESTASISTRAP"),new InputOutputColumnPair(@"SELF_PROTOSS_ORACLE", @"SELF_PROTOSS_ORACLE"),new InputOutputColumnPair(@"SELF_PROTOSS_OBSERVER", @"SELF_PROTOSS_OBSERVER"),new InputOutputColumnPair(@"SELF_PROTOSS_NEXUS", @"SELF_PROTOSS_NEXUS"),new InputOutputColumnPair(@"SELF_PROTOSS_MOTHERSHIPCORE", @"SELF_PROTOSS_MOTHERSHIPCORE"),new InputOutputColumnPair(@"SELF_PROTOSS_MOTHERSHIP", @"SELF_PROTOSS_MOTHERSHIP"),new InputOutputColumnPair(@"SELF_PROTOSS_INTERCEPTOR", @"SELF_PROTOSS_INTERCEPTOR"),new InputOutputColumnPair(@"SELF_PROTOSS_IMMORTAL", @"SELF_PROTOSS_IMMORTAL"),new InputOutputColumnPair(@"SELF_PROTOSS_HIGHTEMPLAR", @"SELF_PROTOSS_HIGHTEMPLAR"),new InputOutputColumnPair(@"SELF_PROTOSS_TWILIGHTCOUNCIL", @"SELF_PROTOSS_TWILIGHTCOUNCIL"),new InputOutputColumnPair(@"ENEMY_PROTOSS_ASSIMILATORRICH", @"ENEMY_PROTOSS_ASSIMILATORRICH")}))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"ProxyDetected",@"MapName",@"ProxyActive",@"ENEMY_PROTOSS_IMMORTAL",@"ENEMY_PROTOSS_HIGHTEMPLAR",@"ENEMY_PROTOSS_GATEWAY",@"ENEMY_PROTOSS_FORGE",@"ENEMY_PROTOSS_FLEETBEACON",@"ENEMY_PROTOSS_DISRUPTORPHASED",@"ENEMY_PROTOSS_DISRUPTOR",@"ENEMY_PROTOSS_DARKTEMPLAR",@"ENEMY_PROTOSS_DARKSHRINE",@"ENEMY_PROTOSS_INTERCEPTOR",@"ENEMY_PROTOSS_CYBERNETICSCORE",@"ENEMY_PROTOSS_CARRIER",@"ENEMY_PROTOSS_ASSIMILATOR",@"ENEMY_PROTOSS_ARCHON",@"ENEMY_PROTOSS_ADEPTPHASESHIFT",@"ENEMY_PROTOSS_ADEPT",@"SELF_PROTOSS_ASSIMILATORRICH",@"SELF_PROTOSS_ZEALOT",@"SELF_PROTOSS_WARPPRISMPHASING",@"SELF_PROTOSS_WARPPRISM",@"ENEMY_PROTOSS_COLOSSUS",@"ENEMY_PROTOSS_MOTHERSHIP",@"ENEMY_PROTOSS_MOTHERSHIPCORE",@"ENEMY_PROTOSS_NEXUS",@"ENEMY_PROTOSS_WARPPRISMPHASING",@"ENEMY_PROTOSS_WARPPRISM",@"ENEMY_PROTOSS_WARPGATE",@"ENEMY_PROTOSS_VOIDRAY",@"ENEMY_PROTOSS_TWILIGHTCOUNCIL",@"ENEMY_PROTOSS_TEMPLARARCHIVE",@"ENEMY_PROTOSS_TEMPEST",@"ENEMY_PROTOSS_STARGATE",@"ENEMY_PROTOSS_STALKER",@"ENEMY_PROTOSS_SHIELDBATTERY",@"ENEMY_PROTOSS_SENTRY",@"ENEMY_PROTOSS_ROBOTICSFACILITY",@"ENEMY_PROTOSS_ROBOTICSBAY",@"ENEMY_PROTOSS_PYLONOVERCHARGED",@"ENEMY_PROTOSS_PYLON",@"ENEMY_PROTOSS_PROBE",@"ENEMY_PROTOSS_PHOTONCANNON",@"ENEMY_PROTOSS_PHOENIX",@"ENEMY_PROTOSS_ORACLESTASISTRAP",@"ENEMY_PROTOSS_ORACLE",@"ENEMY_PROTOSS_OBSERVER",@"SELF_PROTOSS_WARPGATE",@"ENEMY_PROTOSS_ZEALOT",@"SELF_PROTOSS_VOIDRAY",@"SELF_PROTOSS_TEMPLARARCHIVE",@"SELF_PROTOSS_DISRUPTOR",@"SELF_PROTOSS_DARKTEMPLAR",@"SELF_PROTOSS_DARKSHRINE",@"SELF_PROTOSS_CYBERNETICSCORE",@"SELF_PROTOSS_COLOSSUS",@"SELF_PROTOSS_CARRIER",@"SELF_PROTOSS_ASSIMILATOR",@"SELF_PROTOSS_ARCHON",@"SELF_PROTOSS_ADEPTPHASESHIFT",@"SELF_PROTOSS_DISRUPTORPHASED",@"SELF_PROTOSS_ADEPT",@"Frame",@"MyRace",@"MySelectedRace",@"EnemyRace",@"EnemySelectedRace",@"SELF_PROTOSS_FLEETBEACON",@"SELF_PROTOSS_FORGE",@"SELF_PROTOSS_GATEWAY",@"SELF_PROTOSS_TEMPEST",@"SELF_PROTOSS_STARGATE",@"SELF_PROTOSS_STALKER",@"SELF_PROTOSS_SHIELDBATTERY",@"SELF_PROTOSS_SENTRY",@"SELF_PROTOSS_ROBOTICSFACILITY",@"SELF_PROTOSS_ROBOTICSBAY",@"SELF_PROTOSS_PYLONOVERCHARGED",@"SELF_PROTOSS_PYLON",@"SELF_PROTOSS_PROBE",@"SELF_PROTOSS_PHOTONCANNON",@"SELF_PROTOSS_PHOENIX",@"SELF_PROTOSS_ORACLESTASISTRAP",@"SELF_PROTOSS_ORACLE",@"SELF_PROTOSS_OBSERVER",@"SELF_PROTOSS_NEXUS",@"SELF_PROTOSS_MOTHERSHIPCORE",@"SELF_PROTOSS_MOTHERSHIP",@"SELF_PROTOSS_INTERCEPTOR",@"SELF_PROTOSS_IMMORTAL",@"SELF_PROTOSS_HIGHTEMPLAR",@"SELF_PROTOSS_TWILIGHTCOUNCIL",@"ENEMY_PROTOSS_ASSIMILATORRICH"}))      
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(@"Result", @"Result"))      
                                    .Append(mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator:mlContext.BinaryClassification.Trainers.FastTree(new FastTreeBinaryTrainer.Options(){NumberOfLeaves=183,MinimumExampleCountPerLeaf=2,NumberOfTrees=2641,MaximumBinCountPerFeature=8,LearningRate=1F,FeatureFraction=0.431438326447041F,LabelColumnName=@"Result",FeatureColumnName=@"Features"}), labelColumnName: @"Result"))      
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(@"PredictedLabel", @"PredictedLabel"));

            return pipeline;
        }
    }
}
