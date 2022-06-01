using LINQtoCSV;
using Microsoft.ML.Data;

namespace BuildPrediction
{
    public class FlatFrameData
    {
        public float Result;

        //[NoColumn] public string? EnemyId;
        public string? MapName;
        //[NoColumn] public string? Build;

        public float EnemySelectedRace;
        public float EnemyRace;
        public float MySelectedRace;
        public float MyRace;

        public float Frame;

        public bool ProxyDetected;
        public bool ProxyActive;

        //[NoColumn] public float SELF_PROTOSS_ADEPT;
        //[NoColumn] public float SELF_PROTOSS_ADEPTPHASESHIFT;
        //[NoColumn] public float SELF_PROTOSS_ARCHON;
        //[NoColumn] public float SELF_PROTOSS_ASSIMILATOR;
        //[NoColumn] public float SELF_PROTOSS_CARRIER;
        //[NoColumn] public float SELF_PROTOSS_COLOSSUS;
        //[NoColumn] public float SELF_PROTOSS_CYBERNETICSCORE;
        //[NoColumn] public float SELF_PROTOSS_DARKSHRINE;
        //[NoColumn] public float SELF_PROTOSS_DARKTEMPLAR;
        //[NoColumn] public float SELF_PROTOSS_DISRUPTOR;
        //[NoColumn] public float SELF_PROTOSS_DISRUPTORPHASED;
        //[NoColumn] public float SELF_PROTOSS_FLEETBEACON;
        //[NoColumn] public float SELF_PROTOSS_FORGE;
        //[NoColumn] public float SELF_PROTOSS_GATEWAY;
        //[NoColumn] public float SELF_PROTOSS_HIGHTEMPLAR;
        //[NoColumn] public float SELF_PROTOSS_IMMORTAL;
        //[NoColumn] public float SELF_PROTOSS_INTERCEPTOR;
        //[NoColumn] public float SELF_PROTOSS_MOTHERSHIP;
        //[NoColumn] public float SELF_PROTOSS_MOTHERSHIPCORE;
        //[NoColumn] public float SELF_PROTOSS_NEXUS;
        //[NoColumn] public float SELF_PROTOSS_OBSERVER;
        //[NoColumn] public float SELF_PROTOSS_ORACLE;
        //[NoColumn] public float SELF_PROTOSS_ORACLESTASISTRAP;
        //[NoColumn] public float SELF_PROTOSS_PHOENIX;
        //[NoColumn] public float SELF_PROTOSS_PHOTONCANNON;
        //[NoColumn] public float SELF_PROTOSS_PROBE;
        //[NoColumn] public float SELF_PROTOSS_PYLON;
        //[NoColumn] public float SELF_PROTOSS_PYLONOVERCHARGED;
        //[NoColumn] public float SELF_PROTOSS_ROBOTICSBAY;
        //[NoColumn] public float SELF_PROTOSS_ROBOTICSFACILITY;
        //[NoColumn] public float SELF_PROTOSS_SENTRY;
        //[NoColumn] public float SELF_PROTOSS_SHIELDBATTERY;
        //[NoColumn] public float SELF_PROTOSS_STALKER;
        //[NoColumn] public float SELF_PROTOSS_STARGATE;
        //[NoColumn] public float SELF_PROTOSS_TEMPEST;
        //[NoColumn] public float SELF_PROTOSS_TEMPLARARCHIVE;
        //[NoColumn] public float SELF_PROTOSS_TWILIGHTCOUNCIL;
        //[NoColumn] public float SELF_PROTOSS_VOIDRAY;
        //[NoColumn] public float SELF_PROTOSS_WARPGATE;
        //[NoColumn] public float SELF_PROTOSS_WARPPRISM;
        //[NoColumn] public float SELF_PROTOSS_WARPPRISMPHASING;
        //[NoColumn] public float SELF_PROTOSS_ZEALOT;
        //[NoColumn] public float SELF_PROTOSS_ASSIMILATORRICH;

        public float ENEMY_PROTOSS_ADEPT;
        public float ENEMY_PROTOSS_ADEPTPHASESHIFT;
        public float ENEMY_PROTOSS_ARCHON;
        public float ENEMY_PROTOSS_ASSIMILATOR;
        public float ENEMY_PROTOSS_CARRIER;
        public float ENEMY_PROTOSS_COLOSSUS;
        public float ENEMY_PROTOSS_CYBERNETICSCORE;
        public float ENEMY_PROTOSS_DARKSHRINE;
        public float ENEMY_PROTOSS_DARKTEMPLAR;
        public float ENEMY_PROTOSS_DISRUPTOR;
        public float ENEMY_PROTOSS_DISRUPTORPHASED;
        public float ENEMY_PROTOSS_FLEETBEACON;
        public float ENEMY_PROTOSS_FORGE;
        public float ENEMY_PROTOSS_GATEWAY;
        public float ENEMY_PROTOSS_HIGHTEMPLAR;
        public float ENEMY_PROTOSS_IMMORTAL;
        public float ENEMY_PROTOSS_INTERCEPTOR;
        public float ENEMY_PROTOSS_MOTHERSHIP;
        public float ENEMY_PROTOSS_MOTHERSHIPCORE;
        public float ENEMY_PROTOSS_NEXUS;
        public float ENEMY_PROTOSS_OBSERVER;
        public float ENEMY_PROTOSS_ORACLE;
        public float ENEMY_PROTOSS_ORACLESTASISTRAP;
        public float ENEMY_PROTOSS_PHOENIX;
        public float ENEMY_PROTOSS_PHOTONCANNON;
        public float ENEMY_PROTOSS_PROBE;
        public float ENEMY_PROTOSS_PYLON;
        public float ENEMY_PROTOSS_PYLONOVERCHARGED;
        public float ENEMY_PROTOSS_ROBOTICSBAY;
        public float ENEMY_PROTOSS_ROBOTICSFACILITY;
        public float ENEMY_PROTOSS_SENTRY;
        public float ENEMY_PROTOSS_SHIELDBATTERY;
        public float ENEMY_PROTOSS_STALKER;
        public float ENEMY_PROTOSS_STARGATE;
        public float ENEMY_PROTOSS_TEMPEST;
        public float ENEMY_PROTOSS_TEMPLARARCHIVE;
        public float ENEMY_PROTOSS_TWILIGHTCOUNCIL;
        public float ENEMY_PROTOSS_VOIDRAY;
        public float ENEMY_PROTOSS_WARPGATE;
        public float ENEMY_PROTOSS_WARPPRISM;
        public float ENEMY_PROTOSS_WARPPRISMPHASING;
        public float ENEMY_PROTOSS_ZEALOT;
        public float ENEMY_PROTOSS_ASSIMILATORRICH;
    }
}
