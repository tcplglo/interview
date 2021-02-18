using Dapper;
using generic_api.Model.Auth;
using System;

namespace generic_api.Data
{
    public interface IUserRepository
    {
        void CreateAppUser(AppIdentity user, string description);
        AppUser GetAppUserByAppIdentityId(AppIdentity user);
        void RemoveExpiredTokensForUser(AppUser appUser);
        void AddRefreshtokenToUser(AppUser appUser, string refreshToken);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString) { }

        public void CreateAppUser(AppIdentity user, string description)
        {
            const string statement = @"
                                        INSERT INTO [dbo].[AppUsers] ([id],[AspNetUserId],[Excerpt])
                                             VALUES
                                                   (@id
                                                   ,@appId
                                                   ,@desc)                            
                                        ";

            using (var conn = GetConn())
            {
                conn.Execute(statement, new { @id = user.Id, @appId = user.Id, @desc = description });
            }
        }

        public AppUser GetAppUserByAppIdentityId(AppIdentity user)
        {
            const string statement = @"
                            select * 
                              from [AppUsers] us with(nolock)
                             where [id] = @userId";

            using (var conn = GetConn())
            {
                return conn.QueryFirst<AppUser>(statement, new { @userId = user.Id });
            }
        }

        public void RemoveExpiredTokensForUser(AppUser appUser)
        {
            // do nothing for now...
        }

        public void AddRefreshtokenToUser(AppUser appUser, string refreshToken)
        {
            const string statement = @"
                                        INSERT INTO [dbo].[RefreshTokens]
                                                   ([Id]
                                                   ,[AppUserId]
                                                   ,[Created]
                                                   ,[Modified]
                                                   ,[Token]
                                                   ,[Expires])
                                             VALUES
                                                   (@id
                                                   ,@appId
                                                   ,@created
                                                   ,@modified
                                                   ,@token
                                                   ,@expires)
                                        ";

            using (var conn = GetConn())
            {
                conn.Execute(statement, new { @id = Guid.NewGuid()
                                            , @appId = appUser.id
                                            , @created = DateTime.UtcNow
                                            , @modified = DateTime.UtcNow
                                            , @token = refreshToken
                                            , @expires = DateTime.UtcNow.AddMinutes(10)
                });
            }
        }

    }
}
