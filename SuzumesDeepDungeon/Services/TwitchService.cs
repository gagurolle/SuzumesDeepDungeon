using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO.Twitch;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Models.Twitch;
using System;

namespace SuzumesDeepDungeon.Services
{
    public class TwitchService
    {

        private readonly ILogger<RawgApi> _logger;
        private readonly DatabaseContext _context;
        public TwitchService(ILogger<RawgApi> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Проверить есть ли такой пользователь в базе и добавить если пользователя нет 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        public async Task<TwitchUser> CheckUser(string? username = null, string? userId = null, UserInfoEx? user = null)
        {

            if (user!=null)
            {

                var resultUserById = await _context.TwitchUsers.Where(x => x.UserId == user.UserId).FirstOrDefaultAsync();
                if (resultUserById != null)
                {
                    //check consistent data
                    return resultUserById;
                }
                else
                {
                    var t = new TwitchUser()
                    { 
                        UserId = user.UserId!,
                        UserName = username,
                        AccountAge = user.AccountAge,
                        ChannelTitle = user.ChannelTitle,
                        CreatedAt = user.CreatedAt,
                        Description = user.Description,
                        Game = user.Game,
                        GameId = user.GameId,
                        IsFollowing = user.IsFollowing,
                        IsModerator = user.IsModerator,
                        IsSubscribed = user.IsSubscribed,
                        IsVip = user.IsVip,
                        LastActive = user.LastActive,
                        PreviousActive = user.PreviousActive,
                        ProfileImageUrl = user.ProfileImageUrl,
                        SubscriptionTier = user.SubscriptionTier,
                        UserLogin = user.UserLogin,
                        UserType = user.UserType,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow
                    };
                    var dp = _context.TwitchUsers.AddAsync(t);
                    await _context.SaveChangesAsync();

                    return t;
                }
            }
            else
            {
                //обработка для других параметров
                throw new Exception("No User Data");
            }
        }

        public async Task<TwitchSystemAction> CheckAction(TwitchSystemActionDTO? action = null)
        {

            var resultActionById = await _context.TwitchSystemActions.Where(x => x.ActionId == action.ActionId).FirstOrDefaultAsync();
            if (resultActionById == null)
            {
                return await AddTwitchSystemAction(action);

            }
            else
            {
                return resultActionById;
            }
        }


        public async Task<TwitchSystemAction> AddTwitchSystemAction(TwitchSystemActionDTO? action)
        {
            var actionSys = new TwitchSystemAction
            {
                ActionId = action.ActionId,
                ActionName = action.ActionName,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };


            await _context.TwitchSystemActions.AddAsync(actionSys);

            await _context.SaveChangesAsync();
            return actionSys;
        }

        public async void GetTwitchSystemActions()
        {
        }
        public async void DeleteTwitchSystemAction()
        {
        }
        public async void UpdateTwitchSystemAction()
        {
        }

        public async Task AddGameToList(AddGameInput gameInput)
        {
            
                using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                TwitchUser user = null!;
            TwitchSystemAction sysAction = null!;
            TwitchCommandTriggered twitchCommandTriggered = null!;
            TwitchRewardRedemption twitchRewardRedemption = null!;

            if (gameInput.userInfoEx != null)
            {
                user = await CheckUser(user: gameInput.userInfoEx);
            }
                else
                {
                    user = await CheckUser(user: new UserInfoEx { UserLogin = "TestUserSystemNull", UserId = "0000",
                    UserType = "affilate"
                    });
                }
            if (gameInput.action != null)
                {
                    sysAction = await CheckAction(gameInput.action);
                }
            await _context.SaveChangesAsync();
            if (gameInput.twitchRewardRedemption != null)
            {
                twitchRewardRedemption = new TwitchRewardRedemption()
                {
                    SystemAction = sysAction,
                    User = user,
                    RewardCost = gameInput.twitchRewardRedemption.rewardCost,
                    RewardPrompt = gameInput.twitchRewardRedemption.rewardPrompt,
                    Counter = gameInput.twitchRewardRedemption.counter,
                    UserCounter = gameInput.twitchRewardRedemption.userCounter,
                    RewardName = gameInput.twitchRewardRedemption.rewardName,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    UserId = user.Id,
                    SystemActionId = sysAction.Id,

                };
                _context.TwitchRewardRedemptions.Add(twitchRewardRedemption);
            }
            if (gameInput.commandTriggered != null)
            {
                twitchCommandTriggered = new TwitchCommandTriggered()
                {
                    SystemAction = sysAction,
                    User = user,
                    Command = gameInput.commandTriggered.command,
                    CommandName = gameInput.commandTriggered.commandName,
                    CommandSource = gameInput.commandTriggered.commandSource,
                    CommandType = gameInput.commandTriggered.commandType,
                    IsReply = gameInput.commandTriggered.isReply,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    UserId = user.Id,
                    SystemActionId = sysAction.Id,
                };
                _context.TwitchCommandTriggereds.Add(twitchCommandTriggered);

            }
            await _context.SaveChangesAsync();
            var twitchAction = new TwitchAction
            {
                UserId = user.Id,
                SystemActionId = sysAction.Id,
                RowData = gameInput.gameName
            };

            if (twitchCommandTriggered != null)
            {
                twitchAction.CommandTriggeredId = twitchCommandTriggered.Id;
            }

            if (twitchRewardRedemption != null) {
                twitchAction.RewardRedemptionId = twitchRewardRedemption.Id;
                    
                    }

            _context.TwitchActions.Add(twitchAction);


            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            _logger.LogWarning($"TwitchAction создан с ID: {twitchAction.Id}");
        }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
                _logger.LogError($"Ошибка при создании TwitchAction: {ex.Message}");
        throw;
    }
}

        public async void DeleteGameFromList()
        {
        }

        public async Task<List<TwitchAction>> GetGamesFromList()
        {
           var result = await _context.TwitchActions.Include(x => x.User).Include(p => p.CommandTriggered).Include(p => p.RewardRedemption).Include(p => p.SystemAction).ToListAsync();
            return result;
        }
    }
}
