using System;
using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;

namespace MobaManager.Match
{
    public sealed class RuntimeMatchState
    {
        public RuntimeMatchState(
            string instanceId,
            MatchData config,
            RuntimeMapModel map,
            RuntimeTeamState blueTeam,
            RuntimeTeamState redTeam)
        {
            InstanceId = instanceId;
            Config = config;
            Map = map;
            BlueTeam = blueTeam;
            RedTeam = redTeam;
            TimeLimit = config.matchTimeLimit;
            ElapsedTime = 0f;
            IsStarted = false;
            IsCompleted = false;
        }

        public string InstanceId { get; }
        public MatchData Config { get; }
        public RuntimeMapModel Map { get; }
        public RuntimeTeamState BlueTeam { get; }
        public RuntimeTeamState RedTeam { get; }
        public IReadOnlyList<RuntimeHeroState> BlueHeroStates => BlueTeam.HeroStates;
        public IReadOnlyList<RuntimeHeroState> RedHeroStates => RedTeam.HeroStates;
        public IReadOnlyList<RuntimeHeroState> AllHeroStates => BlueHeroStates.Concat(RedHeroStates).ToArray();
        public float TimeLimit { get; }
        public float ElapsedTime { get; private set; }
        public bool IsStarted { get; private set; }
        public bool IsCompleted { get; private set; }
        public int TotalHeroStateCount => BlueHeroStates.Count + RedHeroStates.Count;

        public IReadOnlyList<RuntimePositionState> GetAllPositionStates()
        {
            return AllHeroStates
                .Select(hero => hero.Position)
                .Concat(RuntimePositionStateFactory.CreateMapStructurePositions(Map))
                .ToArray();
        }

        public IReadOnlyList<RuntimeHealthState> GetAllHealthStates()
        {
            return AllHeroStates
                .Select(hero => hero.Health)
                .Concat(RuntimeHealthStateFactory.CreateMapStructureHealthStates(Map))
                .ToArray();
        }

        public IReadOnlyList<RuntimeResourceState> GetAllResourceStates()
        {
            return AllHeroStates
                .Select(hero => hero.Resource)
                .ToArray();
        }

        public RuntimeUnitRegistry CreateUnitRegistry()
        {
            return RuntimeUnitRegistryFactory.Create(this);
        }

        public void Start()
        {
            if (IsCompleted)
            {
                return;
            }

            IsStarted = true;
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime), "Delta time cannot be negative.");
            }

            if (!IsStarted || IsCompleted)
            {
                return;
            }

            ElapsedTime += deltaTime;
            if (ElapsedTime >= TimeLimit)
            {
                ElapsedTime = TimeLimit;
                IsCompleted = true;
            }
        }
    }

    public static class RuntimeMatchStateFactory
    {
        public static RuntimeMatchState Create(
            string instanceId,
            MatchData config,
            RuntimeMapModel map,
            RuntimeTeamState blueTeam,
            RuntimeTeamState redTeam)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (blueTeam == null)
            {
                throw new ArgumentNullException(nameof(blueTeam));
            }

            if (redTeam == null)
            {
                throw new ArgumentNullException(nameof(redTeam));
            }

            RuntimeMatchState matchState = new RuntimeMatchState(
                instanceId,
                config,
                map,
                blueTeam,
                redTeam);

            InitializeHeroPositions(matchState);
            return matchState;
        }

        public static RuntimeMatchState CreateDemoState(
            RuntimeEntityModelDatabase runtimeDatabase,
            GameDataDatabase gameDataDatabase)
        {
            MatchData config = gameDataDatabase.MatchesById.Values.FirstOrDefault();
            if (config == null)
            {
                throw new InvalidOperationException("Cannot create match state without match config.");
            }

            RuntimeMapModel map = runtimeDatabase.MapsById.Values.FirstOrDefault();
            if (map == null)
            {
                throw new InvalidOperationException("Cannot create match state without map model.");
            }

            RuntimeTeamState blueTeam = RuntimeTeamStateFactory.CreateDemoState("blue", gameDataDatabase, runtimeDatabase);
            RuntimeTeamState redTeam = RuntimeTeamStateFactory.CreateDemoState("red", gameDataDatabase, runtimeDatabase);

            return Create(
                $"demo_{config.id}",
                config,
                map,
                blueTeam,
                redTeam);
        }

        private static void InitializeHeroPositions(RuntimeMatchState matchState)
        {
            RuntimePoint blueStart = FindNexusPosition(matchState.Map, "blue");
            RuntimePoint redStart = FindNexusPosition(matchState.Map, "red");

            foreach (RuntimeHeroState heroState in matchState.BlueHeroStates)
            {
                heroState.SetPosition(blueStart);
            }

            foreach (RuntimeHeroState heroState in matchState.RedHeroStates)
            {
                heroState.SetPosition(redStart);
            }
        }

        private static RuntimePoint FindNexusPosition(RuntimeMapModel map, string team)
        {
            RuntimeNexusModel nexus = map.Nexuses.FirstOrDefault(item => item.Team == team);
            return nexus == null ? new RuntimePoint(0f, 0f) : nexus.Position;
        }
    }
}
