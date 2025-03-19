namespace InsternShip.Common
{
    public static class ExceptionMessages
    {
        public const string QuestionNotFound = "Question is not found.";
        public const string FileNotFound = "File is not found.";
        public const string QuestionTagNotFound = "Question Tag is not found.";
        public const string QBankNotFound = "No QuestionBank entry found";
        public const string DontHaveTag = "Question does not have a tag.";
        public const string DataNotFound = "Data not found.";
        public const string FieldIsRequired = "Required field(s) is/are empty.";
        public const string ExcelFileRequired = "Only .xls and .xlsx files are allowed.";
        public const string WrongPassword = "Wrong Password.";
        public const string PasswordMismatch = "Confirm Password doesn't match Password.";
        public const string TestNotFound = "Test is not found.";
        public const string TestNotDeleted = "Test is not deleted.";
        public const string DuplicateQuestion = "Question is already in Test.";
        public const string RecruiterExisted = "Recruiter existed.";
        public const string UnexpectedException = "There an unexpected error.";
        public const string CandidateNotFound = "Candidate is not found.";
        public const string CandidateExisted = "Candidate existed.";
        public const string CandidateNotDeleted = "Candidate is not deleted.";
        public const string InterviewerNotDeleted = "Interviewer is not deleted.";
        public const string StatusNotExist = "Status does not exist.";
        public const string RecruiterNotFound = "Recruiter is not found.";
        public const string RecruiterNotDeleted = "Recruiter is not deleted.";
        public const string RecruiterJobPostNotFound = "Job post is not found.";
        public const string JobPostNotDeleted = "Job post is not deleted.";
        public const string RecruiterEventPostNotFound = "Event post is not found.";
        public const string EventPostNotDeleted = "Event post is not found.";
        public const string UserNotFound = "User not found";
        public const string UserInfoNotFound = "User info not found";
        public const string InterviewerNotFound = "Interviewer is not found";
        public const string InterviewNotFound = "Interview is not found";
        public const string InterviewNotDeleted = "Interview is not deleted.";
        public const string InterviewerExisted = "Interviewer existed.";
        public const string ApplicationNotFound = "Application is not found";
        public const string EventNotFound = "Event is not found.";
        public const string EventMaxCandidate = "Maximum number of candidates registered";
        public const string EventParticipationNotFound = "EventParticipation is not found.";
        public const string EventParticipationFound = "Candidate has already participated.";
        public const string JobNotFound = "Job is not found.";
        public const string HaveNotQTag = "This Question have no Tag.";
        public const string AlreadyHaveQTag = "This Question Already have a Tag.";
        public const string UserNotBanned = "User is not banned.";
        public const string BlackListNotFound = "Blacklist Entry not found.";
        public const string FilterNotFound = "Filter type not found";
        public const string UserAlrBanned = "User is already banned.";
        public const string InterviewSessionNotFound = "Interview session not found";

        public const string RoleNotFound = "Role is not found.";
        public const string RoleClaimsNotFound = "RoleClaims is not found.";
        public const string UserRoleNotFound = "UserRole is not found.";
        public const string AlreadyApplied = "You have already apply for this job";
        public const string AlreadyRejected = "Application already been rejected";
        public const string NotInterviewed = "This Application is not set to Interviewed status.";
        public const string NotOwnInterview = "This is not your interview.";
        public const string AlreadyInInterview = "You have already in an interview with this application.";
        public const string DuplicateSession = "The Interviewer already assigned for this interview";
        public const string RequirementsNotMet = "Requirements not met";
        public const string StatusUpdateNotFound = "Status Update not Found";
        public const string ApplicationNotDeleted = "Application has not been deleted";

        public const string TokenNotFound = "Token is not Found";
        public const string EmailDoesNotExist = "Email does not exist.";
        public const string FillOtherRole = "Please fill in 1 of the following 4 roles ADMIN/CANDIDATE/RECRUITER/INTERVIEWER";
        public const string TokenIsEmpty = "Token is Empty";
        public const string CVNotFound = "CV is not found";
        public const string RoleCreated = "Role has been created";
        public const string UserInforNotFound = "User infor not found";
        public const string RecruiterInInterviewNotFound = "No interview has been scheduled with this employer yet.";

        public const string RecruiterNotPermission = "Recruiter does not have permission to delete";
        public const string NotOwnPost = "This is not from your post(s).";
        public const string EventPostDeadlinePassed = "The event deadline has expired";

        public const string AccountHasBeenBanned = "Account has been banned.";
        public const string AccountHasBeenDeactivated = "Account has been deactivated.";
        public const string AccountHasNotBeenConfirmed = "Account has not been confirmed.";
        public const string WrongEmailOrPassword = "Wrong email or password.";
    }
    public static class SuccessMessages
    {
        public const string Login = "Login success.";
    }

    public static class MissingFieldMessage
    {
        public const string MissingFile = "File is not choose.";
        public const string MissingPublicId = "PublicId is not fill.";
    }

    public static class DuplicateMessage
    {
        public const string DuplicateRoleName = "Role name is already exist.";
        public const string DuplicateRoleClaimsType = "RoleClaims type is already exist.";
        public const string DuplicateUserRole = "UserRole is already exist.";
        public const string AccountConfirmed = "Your account has been confirmed.";

        //public const string DuplicateUserRole = "User has already role";
    }
}
