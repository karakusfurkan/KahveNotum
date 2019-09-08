
namespace KahveNotum.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,
        UserIsNotActive = 151,
        UsernameorPassWrong = 152,
        CheckYourEmail = 153,
        UserAlreadyActive = 154,
        ActivateIDDoesNotExist = 155,
        UserNotFound = 199,
        UserCouldNotUpdated = 200,
        UserCouldNotRemove = 201,
        UserCouldNotFind = 202,
        UserCouldNotInserted = 203,
    }
}
