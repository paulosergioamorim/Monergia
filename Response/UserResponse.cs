using Monergia.Models;

namespace Monergia.Response;

public class UserResponse
{
    private readonly User _user;

    public UserResponse(User user) => _user = user;

    public Guid Id => _user.Id;

    public string Name => _user.Name;

    public DateOnly BirthDate => _user.BirthDate;

    public string Username => _user.Username;

    public string Role => _user.Role;

    public Guid FilialId => _user.FilialId;
}