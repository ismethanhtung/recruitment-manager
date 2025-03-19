namespace InsternShip.Api.Claims.System
{
    public static class Authority
    {
        #region Admin
        public const string ADMIN_ROLE_VIEW = "Admin.Role.View";
        public const string ADMIN_ROLE_EDIT = "Admin.Role.Edit";
        public static readonly List<string> LIST_ADMIN_CLAIMS = new()
        {
            ADMIN_ROLE_VIEW,
            ADMIN_ROLE_EDIT
        };
        #endregion

        #region Candidate
        public const string CANDIDATE_CV_EDIT = "Candidate";
        public static readonly List<string> LIST_CANDIDATE_CLAIMS = new()
        {
            CANDIDATE_CV_EDIT
        };
        #endregion

        #region Recruiter
        public const string RECRUITER_JOB_POST_VIEW = "Recruiter.JobPost.View";
        public const string RECRUITER_JOB_POST_EDIT = "Recruiter.JobPost.Edit";
        public static readonly List<string> LIST_RECRUITER_CLAIMS = new()
        {
            RECRUITER_JOB_POST_VIEW,
            RECRUITER_JOB_POST_EDIT
        };
        #endregion

        #region Interviewer
        public const string INTERVIEWER_QUESTION_VIEW = "Interviewer.Question.View";
        public const string INTERVIEWER_QUESTION_EDIT = "Interviewer.Question.Edit";
        public static readonly List<string> LIST_INTERVIEWER_CLAIMS = new()
        {
            INTERVIEWER_QUESTION_VIEW,
            INTERVIEWER_QUESTION_EDIT
        };
        #endregion
    }
}
