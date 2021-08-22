using System;
using System.Collections.Generic;
using System.Linq;

using GameSpace.Data;
using GameSpace.Services.Algorithms.Contracts;
using GameSpace.Services.Algorithms.Models;
using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Algorithms
{
    public class AlgorithmService : IAlgorithmService
    {
        private readonly GameSpaceDbContext data;

        public AlgorithmService(GameSpaceDbContext data)
            => this.data = data;

        public SingleEliminationSeedsAlgorithmServiceModel SingleEliminationFirstRoundSeeds(
            List<IdNamePairTeamServiceModel> teamIdNamePairs)
        {
            var participantsCount = teamIdNamePairs.Count;

            var roundsCount = GetRoundsCount(participantsCount);

            var result = new SingleEliminationSeedsAlgorithmServiceModel
            {
                CountOfRounds = roundsCount
            };

            var seedsCountFirstRound = GetSeedsCount(participantsCount);

            var teamsIds = teamIdNamePairs
                            .Select(t => t.Id)
                            .ToList();


            var teamsSeeds = result.TeamsSeeds;

            var random = new Random();

            PopulateFirstRound(teamIdNamePairs, seedsCountFirstRound, teamsIds, teamsSeeds, random);

            return result;
        }

        public SingleEliminationSeedsAlgorithmServiceModel SingleEliminationSecondRound(
            SingleEliminationSeedsAlgorithmServiceModel teamIdNamePairs)
        {
            List<SeedingsTeamServiceModel> teamsSeeds = teamIdNamePairs.TeamsSeeds;

            var seed = 1;

            for (var i = 1; i <= teamIdNamePairs.TeamsSeeds.Count; i = i + 2)
            {
                var blueTeam = teamsSeeds[i - 1];
                var redTeam = teamsSeeds[i];

                if (blueTeam.IsEliminated)
                {
                    if (blueTeam.SeedMaches.Count >= 2) //redaction logic
                    {
                        for (int y = 1; y < blueTeam.SeedMaches.Count; i++)
                        {
                            redTeam.SeedMaches.Add(blueTeam.SeedMaches[y]);

                            blueTeam.SeedMaches.RemoveAt(y);
                        }
                    }
                    else
                    {
                        if (redTeam.SeedMaches.Count < 2)
                        {
                            redTeam.SeedMaches.Add(GetSeedByPrevious(i));
                        }
                    }
                }

                if (redTeam.IsEliminated)
                {
                    if (redTeam.SeedMaches.Count >= 2) //redaction logic
                    {
                        for (int y = 1; y < redTeam.SeedMaches.Count; i++)
                        {
                            blueTeam.SeedMaches.Add(redTeam.SeedMaches[y]);

                            redTeam.SeedMaches.RemoveAt(y);
                        }
                    }
                    else
                    {
                        if (blueTeam.SeedMaches.Count < 2)
                        {
                            blueTeam.SeedMaches.Add(GetSeedByPrevious(seed));
                        }
                    }
                }

                seed++;
            }

            return teamIdNamePairs;
        }

        public SingleEliminationSeedsAlgorithmServiceModel SingleEliminationThirdRound(
            SingleEliminationSeedsAlgorithmServiceModel teamIdNamePairs)
        {
            List<SeedingsTeamServiceModel> teamsSeeds = teamIdNamePairs.TeamsSeeds;

            for (var i = 0; i < teamIdNamePairs.TeamsSeeds.Count; i++)
            {
                var blueTeam = teamsSeeds[i];

                if (blueTeam.SeedMaches.Count < 2)
                {
                    continue;
                }

                if (blueTeam.IsEliminated)
                {
                    var seed = blueTeam.SeedMaches[1];

                    var redTeam = teamIdNamePairs.TeamsSeeds
                        .FirstOrDefault(t =>
                        t.SeedMaches.Count == 2 &&
                        t.teamIdNamePair.Name != blueTeam.teamIdNamePair.Name &&
                        t.IsEliminated == false &&
                        t.SeedMaches[1] == seed);

                    //var redTeam = 

                    if (blueTeam.SeedMaches.Count >= 3) //redaction logic
                    {
                        for (int y = 2; y < blueTeam.SeedMaches.Count; i++)
                        {
                            redTeam.SeedMaches.Add(blueTeam.SeedMaches[y]);

                            blueTeam.SeedMaches.RemoveAt(y);
                        }
                    }
                    else
                    {
                        redTeam.SeedMaches.Add(GetSeedByPrevious(seed));
                    }
                }
            }

            return teamIdNamePairs;
        }

        public SingleEliminationSeedsAlgorithmServiceModel SingleEliminationNextRound(
            SingleEliminationSeedsAlgorithmServiceModel previousTeamIdNamePairs, int wantedRound)
        {
            List<SeedingsTeamServiceModel> teamsSeeds = previousTeamIdNamePairs.TeamsSeeds;

            for (var i = 0; i < previousTeamIdNamePairs.TeamsSeeds.Count; i++)
            {
                var blueTeam = teamsSeeds[i];

                if (blueTeam.SeedMaches.Count < wantedRound - 1)
                {
                    continue;
                }

                if (blueTeam.IsEliminated)
                {
                    var seed = blueTeam.SeedMaches[wantedRound - 2];

                    var redTeam = previousTeamIdNamePairs.TeamsSeeds
                        .FirstOrDefault(t =>
                        t.SeedMaches.Count == wantedRound - 1 &&
                        t.teamIdNamePair.Name != blueTeam.teamIdNamePair.Name &&
                        t.IsEliminated == false &&
                        t.SeedMaches[wantedRound - 2] == seed);

                    //var redTeam = 

                    if (blueTeam.SeedMaches.Count >= wantedRound) //redaction logic
                    {
                        for (int y = wantedRound - 1; y < blueTeam.SeedMaches.Count; i++)
                        {
                            redTeam.SeedMaches.Add(blueTeam.SeedMaches[y]);

                            blueTeam.SeedMaches.RemoveAt(y);
                        }
                    }
                    else
                    {
                        redTeam.SeedMaches.Add(GetSeedByPrevious(seed));
                    }
                }
            }

            return previousTeamIdNamePairs;
        }
        private int GetSeedByPrevious(int prevSeed)
        {
            return (prevSeed % 2 == 0 ? prevSeed : prevSeed + 1) / 2;
        }

        private int GetRoundsCount(int participantsCount)
        {
            int rounds = default;

            if (participantsCount > 32)
            {
                rounds = 6;
            }
            else if (participantsCount > 16)
            {
                rounds = 5;
            }
            else if (participantsCount > 8)
            {
                rounds = 4;
            }
            else if (participantsCount > 4)
            {
                rounds = 3;
            }
            else
            {
                rounds = 2;
            }

            return rounds;
        }

        private int GetSeedsCount(int participantsCount)
        {
            var result = (decimal)participantsCount / 2;

            var result2 = Math.Ceiling(result);

            return (int)result2;
        }

        private static void PopulateFirstRound(List<IdNamePairTeamServiceModel> teamIdNamePairs, int seedsCountFirstRound, List<int> teamsIds, List<SeedingsTeamServiceModel> teamsSeeds, Random random)
        {
            for (int i = 1; i <= seedsCountFirstRound; i++)
            {
                var teamidIndex = random.Next(0, teamsIds.Count);

                teamsSeeds.Add(new SeedingsTeamServiceModel
                {
                    teamIdNamePair = teamIdNamePairs.FirstOrDefault(t => t.Id == teamsIds[teamidIndex]),
                });

                teamsSeeds
                    .FirstOrDefault(t => t.teamIdNamePair.Id == teamsIds[teamidIndex])
                    .SeedMaches
                    .Add(i);

                teamsIds.RemoveAt(teamidIndex);

                teamidIndex = random.Next(0, teamsIds.Count);

                teamsSeeds.Add(new SeedingsTeamServiceModel
                {
                    teamIdNamePair = teamIdNamePairs.FirstOrDefault(t => t.Id == teamsIds[teamidIndex]),
                });

                teamsSeeds
                    .FirstOrDefault(t => t.teamIdNamePair.Id == teamsIds[teamidIndex])
                    .SeedMaches
                    .Add(i);

                teamsIds.RemoveAt(teamidIndex);
            }
        }
    }
}