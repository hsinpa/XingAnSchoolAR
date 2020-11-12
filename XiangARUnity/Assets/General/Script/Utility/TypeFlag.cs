
public class TypeFlag
{
    public enum UserType {Student, Teacher, Guest };


    public class SocketEvent {
        public const string OnConnect = "OnConnect";
        public const string UpdateUserInfo = "event@update_userInfo";
        public const string UserJoined = "event@user_joined";
        public const string RefreshUserStatus = "event@refresh_user_status";
        public const string UserLeaved = "event@user_leave";
        public const string CreateRoom = "event@create_room";
        public const string StartGame = "event@start_game";
        public const string TerminateGame = "event@force_end_game";
        public const string KickFromGame = "event@kick_from_game";
    }

    public class Audio {
        public const string UITag = "UI";

        public const string EffectFinalScore = "FinalScore";
        public const string EffectCorrect = "Correct";
        public const string EffectWrong= "Wrong";
        public const string EffectChoiceSelect = "ChoiceSelect";
    }

}
